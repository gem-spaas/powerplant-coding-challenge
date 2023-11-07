from powerplant_handler.powerplant_handler import PowerplantHandler
from flask import Flask, request, jsonify
import logging
import configparser

config = configparser.ConfigParser()
config.read('config.ini')

# Set up basic logging configuration
logging.basicConfig(level=logging.INFO, filename='server_logs.log', filemode='a',
                    format='%(asctime)s %(levelname)s:%(name)s:%(message)s')

app = Flask(__name__)


@app.route('/productionplan', methods=['POST'])
def production_plan():
    if request.method == 'POST':
        try:
            payload = request.get_json()
            if not payload:
                app.logger.error('Invalid payload')
                return jsonify({"error": "Invalid payload"}), 400
            response = PowerplantHandler.calculate_production_plan(payload, config)
            return jsonify(response)
        except Exception as e:
            # Here we log the exception
            app.logger.error('Error occurred in production_plan endpoint', exc_info=True)
            return jsonify({"error": str(e)}), 500


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=8888)
