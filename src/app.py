import sys
import os

# Add the src directory to sys.path to ensure modules within src can be found
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), ".")))

from production_plan import calculate_production_plan


from flask import Flask, request, jsonify
from production_plan import calculate_production_plan
import json

app = Flask(__name__)

@app.route('/productionplan', methods=['POST'])
def production_plan():
    try:
        # Retrieve JSON data from the request
        data = request.get_json()
        
        # Calculate the production plan
        result = calculate_production_plan(data)

        # Output file path
        output_filename = 'Merouane_Hadouch_HEADMIND_production_plan_result.json'

        # Save the result in a JSON file in the current directory
        with open(output_filename, 'w', encoding='utf-8') as f:
            json.dump(result, f, indent=4)

        # Return a confirmation message with the file path
        return jsonify({"message": f"The JSON file was successfully generated in the directory with the name '{output_filename}'"}), 200

    except ValueError as e:
        app.logger.error(f"ValueError: {str(e)}")
        return jsonify({"error": str(e)}), 400
    except Exception as e:
        app.logger.error(f"Unexpected error: {str(e)}")
        return jsonify({"error": "An unexpected error occurred."}), 500

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=8888)
