# Tech stack/tools


* black https://github.com/psf/black for code linting
* poetry for dependency pining/management
* django 
* django rest framework for the api request/responce cycle
* channels for the websocket
* docker-compose as dev env

# How to run

First build the docker images with `docker-compose build`
Then spinup the api with `docker-compose up`

# How to run the Tests

First build the images with `docker-compose build`
Then run the tests with `docker-compose run powerplant-challenge ./manage.py test`

# use web socket

The server exposes the websocket on the path `/websocket` and there is a page with some simple js to track the plan requests available on `/monitor`

Everything is exposed on `0.0.0.0:8888`
