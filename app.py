from flask import Flask,request,make_response,jsonify
import json
import logging

ALLOWED_EXTENSIONS = {'json'}
logging.basicConfig(filename='example.log', level=logging.DEBUG)
app = Flask(__name__)

def __allowed_file(filename):
    logging.debug("allowed_file call")
    logging.debug("filename :" + filename)
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

def __get_cost_and_factor(data, powerplant_type):
   # Default return value will be for wind, at 0 cost.
    logging.debug("cost and factor call")
    cost = 0.0
    factor =1
    if powerplant_type == "gasfired":
        logging.debug("gasfired")
        cost = float(data["gas(euro/MWh)"])
    elif powerplant_type == "turbojet":
        logging.debug("turbojet")
        cost = float(data["kerosine(euro/MWh)"])
    elif powerplant_type == "windturbine":
        logging.debug("windtube")    
        factor = float(data["wind(%)"]/100)
    return cost,factor

def __process(data):
  load = data['load']
  remain =load
  #sort on merit order
  logging.debug("Sort powerplants")
  ordered_powerplants = sorted(data['powerplants'], key=lambda item:item['efficiency'], reverse=True)
  for power_plant in ordered_powerplants:
     power_plant["cost"],power_plant["factor"]= __get_cost_and_factor(data['fuels'],power_plant['type'])
     max_gen = power_plant["factor"]* power_plant["pmax"] * power_plant["efficiency"]
     if remain - max_gen > 0:
        power_plant["usage"]= int(max_gen)
        remain -= max_gen
     elif 0 < remain < max_gen:
           power_plant["usage"]= int(remain)
           remain =0
     else:
        power_plant["usage"] = 0
  return [{"name":x["name"],"p":x["usage"]} for x in ordered_powerplants]

#test route
@app.route('/test', methods=['GET'])
def test():
  logging.debug("Testing route.")
  return make_response(jsonify({'message': 'test'}), 200)


@app.route('/productionplan', methods=['POST'])
def productionplan():
  data = request.files
  if 'playload' in data:
    file = data['playload']
    if file and __allowed_file(file.filename):
        content = json.loads(file.read())
        return make_response(jsonify(__process(content)))      
    else:
       logging.error("Wrong file extension")
       return make_response(jsonify({'result':'Incorrect file extension'}))
  else:
       logging.error("File not found")
       return make_response(jsonify({'result':'File not found.'}))