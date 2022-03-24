
1. Prerequisites
------------------
Please make sure you have dotnet core installed on the machine you are running

	1.1. In case you need to to download https://dotnet.microsoft.com/en-us/download
	1.2. to test the dotnet environment please run this command dotnet --version you should see
		> dotnet --version
		> 6.0.201
	Version higher than 6.0 is proper for the project

2. Running the project
-----------------------

2.1 First option 

	Run Command :	dotnet run --project "<local path to the project>"

				    ***local path example : D:\TechnicalWorkspace\VisualStudio\Challenge\ProductionPlanApi

2.2 Second Option 

	step 1: select the project directory
	step 2: Run command : dotnet run

	If everything is installed correctly you should see in the terminal the following:
		
	Building...
	info: Microsoft.Hosting.Lifetime[14]
      	Now listening on: https://localhost:8888       
	info: Microsoft.Hosting.Lifetime[0]
      	Application started. Press Ctrl+C to shut down.
	info: Microsoft.Hosting.Lifetime[0]
      	Hosting environment: Production
	info: Microsoft.Hosting.Lifetime[0]
      	Content root path: D:\TechnicalWorkspace\VisualStudio\Challenge\ProductionPlanApi\
	
3. Testing the project
-----------------------
Within ProductionPlanApi/Postman you can find a simple Postman test collection to consume the services exposed by the ProductionPlanAPi. Import the ProductionPlan.postman_collection.json into postman to easily test

Application Runs on https://localhost:8888/productionplan

In may be necesarely to generate a new developers certicate to run on localhost 
step1: Clean the development certificates: 
	dotnet dev-certs https --clean
step2:Create a new one: 
	dotnet dev-certs https -t
