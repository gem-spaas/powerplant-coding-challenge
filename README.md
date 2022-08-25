How to run the application 
### Prerequisits
- You should have Docker installed on your machine

### Build your docker image
execute from the root folder of this project

    docker build --tag powerplantapp .  


### Start the docker image
    docker run  -p 8888:8888 powerplantapp


### Test your app
Open Postman and use the payload to test your application : 
http://localhost:8888/productionplan

