from flask import Flask

from blue_print import production_plan_blue_print

app = Flask(__name__)
app.register_blueprint(production_plan_blue_print)

if __name__ == "__main__":
    app.run(host="127.0.0.0", port=8888)
