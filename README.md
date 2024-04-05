# How to use the API ?

## Requirements
- Docker
- Python 3.9
- Postman
- A virtual Python environnement

## With Docker
- In a terminal :
	- Go to root project path
	- `docker build -t myimage .` *replace myimage with the name you want to give it*
	- `docker --rm -p 8888:8888 myimage` *replace myimage with the name you want to give it*
- In Postman :
	- `curl --location 'localhost:8888/productionplan' \
--header 'Content-Type: application/json' \
--data 'myJSON'` *replace myJSON with the Json you want to send*
	- OR
	- `POST /productionplan HTTP/1.1
Host: localhost:8888`

## Without Docker
- In a terminal : 
	- Go to root project path
	- `cd src`
	- `python -m main`
- In Postman :
	- `curl --location 'localhost:8888/productionplan' \
--header 'Content-Type: application/json' \
--data 'myJSON'` *replace myJSON with the Json you want to send*
	- OR
	- `POST /productionplan HTTP/1.1
Host: localhost:8888`

### Julie Kramarczyk - April 2024
