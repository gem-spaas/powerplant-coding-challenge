##Dockerized PowerPlan http://localhost:8888/productionplan  

*note that in docker the application should run on http 


Prerequisites
--------------
Docker installation is required, more information https://www.docker.com/
Test if docker is installed correctly:

docker --version
Docker version 20.10.12, build e91ed57




1. Build the solution 
---------------------
Having docker run: 

	docker build --rm --pull -f "<PATH_OF_THE_PROJECT>/Dockerfile" --label "com.microsoft.created-by=visual-studio-code" -t "productionplanapi:latest" "<PATH_OF_THE_PROJECT>" 

2. Build application
---------------------
Having docker run: 
	
	docker run --rm -it  -p 8888:8888/tcp productionplanapi:latest 


3. Testing the project
-----------------------
Within ProductionPlanApi/Postman you can find a simple Postman test collection to consume the services exposed by the ProductionPlanAPi. Import the ProductionPlan.postman_collection.json into postman to easily test

Application Runs on http://localhost:8888/productionplan

In may be necessary to generate a new developers certificate to run on localhost 
step1: Clean the development certificates: 

	dotnet dev-certs https --clean 

step2:Create a new one: 

	dotnet dev-certs https -t