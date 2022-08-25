from flask import Flask


app = Flask(__name__)



@app.route("/productionplan")
def get_prod_plat():
    return "Hi there"