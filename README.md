# Required

The project is using .NET 8 so we need : 

- .NET 8 SDK is installed
- Visual Studio up to date
- Postman ( to test in the case of using docker )
- Docker

#To start project

We have to ways to start this Porject 

#VisualStudio
1- Using visual studio :
	
	a- Open sln and build project 
	b- Run soultion you willbe redirect to swagger portal
	c- You can put your json to test after clicking in try it button linked to post method in productionplan enpoint.

#Docker Part

2- Using Docker

	a- Run command "docker build -t powerplant ." to build the docker image in cmd in the root of repository.
	b- Launch in cmd command  "docker run -p 8888:8080 powerplant:latest" to run container
	c- you can test API via postman ( for example ) using url: http://localhost:8888/productionplan , method post and json.


#I used for this project design pattern strategy to define algorithm for each family of power center
