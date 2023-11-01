## Powerplane coding Challange (Barnabé Magos)

# How to run the Api 

Clone the repo, then cd into /PowerPlant.Api and run the following command :
dotnet run

Or you can use Docker
 
clone the project
cd into the root and run :

docker build --pull -t power-plant-app .
docker run -it --rm -p 8888:80 power-plant-app

The api (and a swagger) are now available at http://localhost:8888/ | http://localhost:8888/swagger/index.html 

# Improvement 

- Authentication
- InputValidation
- Detailed error response