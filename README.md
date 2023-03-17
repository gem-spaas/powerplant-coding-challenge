# Powerplant coding challenge

This is my implementation of the powerplant coding challenge proposed by GEM team.

## How to run it

To deploy it quickly i propose a docker solution.
First build application by running in directory :

    docker compose build

Secondly run application:

    docker compose up flask_app

## Test it

I used Postman to simulate POST request. Request body awaits a key, value pair where the key is "playload".