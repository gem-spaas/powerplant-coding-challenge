from flask import Flask, request
from methods import calculate_production_plan
from logger import setup_logger
app = Flask(__name__)
logger = setup_logger()
@app.route('/productionplan', methods=['POST'])
def productionplan():
    try:
        payload = request.get_json()
        if not payload:
            payload_issue_message = "Invalid or missing payload"
            logger.warning(payload_issue_message)
            raise ValueError(payload_issue_message)
        logger.info(f"Payload received: {payload}")
        return calculate_production_plan(payload), 200

    except Exception as e:
        exception_message = f"Error while processing payload : {e}"
        logger.error(exception_message)
        return {"error": exception_message}, 500

if __name__ == '__main__':
    app.run(debug=True,port=8888)