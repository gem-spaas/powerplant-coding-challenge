# PowerPlant Production Plan API

## Introduction
The **PowerPlant Production Plan API** is a simple API based on Flask **framework** that helps decide how much power each power plant should make. It uses the cost of fuel,  how efficiewt each plant is, and the total power needed to come up with the best plan.

The `/productionplan` endpoint takes information about the total power needed, fuel costs, and power plant details. It then returns a plan for how much power each plant should produce.

## Requirements
To run this project, you need the following:

- Python 3.11 or higher
- Flask

You can install the required Python packages using the command:
```bash
pip install -r requirements.txt
```

**requirements.txt**:
```
blinker==1.8.2
click==8.1.7
colorama==0.4.6
Flask==3.0.3
iniconfig==2.0.0
itsdangerous==2.2.0
Jinja2==3.1.4
MarkupSafe==2.1.5
packaging==24.1
pluggy==1.5.0
pytest==8.3.3
Werkzeug==3.0.4
```

## Usage
To start the API server, run the following command:
```bash
python app.py
```
By default, the API will be available at `http://127.0.0.1:8888`.

### API Endpoints
- **Endpoint**: `/productionplan`
- **Method**: `POST`
- **Description**: Accepts input data in JSON format and returns a production plan for the power plants.


## Output
The output is a JSON array that specifies how much power each power plant should generate to meet the load.




