# GemSpaasPowerplant

My name is Jean-Pierre Battaille.  
I made this .NET Core 6 start of solution to resolve the gem-spaas power plant coding challenge.  

# Installation

Opening the GemSpaasPowerplant.sln solution in Visual Studio 2022 and running the same name project will make the endpoint available and launch a Swagger in the browser.  

With postman, attacking the endpoint with the following parameters will return a json response.  
&emsp;     endpoint:&emsp;  https://localhost:8888/powerplant  
&emsp;     header:&emsp;    Content-Type: application/json  
&emsp;     Raw: &emsp;     insert one of the json example payload  
&emsp;     settings: &emsp;disabling SSL check will simplify the setup  
       
Currently the algorithm doesn't solve  all examples.  A better linear programming solution should be implemented.
