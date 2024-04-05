from flask import Flask, request
import logging
from Controllers.PowerPlantController import handleJson	 

app = Flask(__name__)

logger = logging.getLogger(__name__)
logging.basicConfig(filename='engie_coding_challenge.log', encoding='utf-8', level=logging.DEBUG)


@app.route('/productionplan', methods=['POST'])
def get_response():
    try:
        return handleJson(request.get_json())
    except Exception:
        logger.error(f"get_response : {Exception}")
        return(f"Routers/app.py/get_response : {Exception}")