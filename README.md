# powerplant-coding-challenge

## Requirements

- Python 3.8 or higher.

- [Insomnia](https://insomnia.rest/) (free) or [Postman](https://www.postman.com/) (paid), for debugging and testing APIs locally.


## Installation

Within the folder containing the project, create a Python virtual environment and activate it:

    python -m venv .env

Activation for Windows users:

    ./.env/Scripts/activate

Activation for Mac and Linux users:

    source .env/bin/activate

Once activated, we can go ahead and install the necessary dependencies:

    pip install -r requirements.txt

## Set up

Once the environment is activated and the requirements are installed, we can set any environment variables within the `.flaskenv` file in the root directory.

In this case, as requested, the default port used is 8888, and so is set in FLASK_RUN_PORT.

We can now start the server by running:

    flask run


## Usage

From one of the API tools mentioned in the requirements section above, go ahead and send a POST request to:

    http://127.0.0.1:8888/productionplan

containing a JSON body with the structure showed in one of the payloads within the `example_payloads` folder.