
# About the API

## Prerequisite

Current API was built throught Flask framework. In order to launch Flask server, some dependencies
are needed, and should be installed (see requirements.txt)

## API using

Of course, you need to launch the flask server. On root folder, launch app.py.

### First way : throught script

Always on the root, a script called *launch_dummy_payload.py* init a request attempt to the server. To use it,
just set the path to your payload.json in the variable *file_path* in the script, then launch it on CLI.

### Second way : curl command

Less convenient, curl command could be use, like this : *curl -X POST http://127.0.0.0:8888/production_plan -H "Content-Type: application/json" -d your_json_values*. Keep in mind that is not the most convenient way for large set of data. In this case, use the first option.

## Interesting upgrade

### CO2 cost inclusion

Could be interesting to add in merit order this cost

### Production quantity order

Include production qty in using order, after cost;
By instance, producing load of 400 with two powerplants (efficienty and cost equals) pmin and pmax respectively at 100/300 and 200/450.
It makes sens to use second before, to avoid two powerplants activation (first cannot take the load completely, second is required,
which oversize the asked load: 300 + 200 > 400).
