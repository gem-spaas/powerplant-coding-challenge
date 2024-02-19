# powerplant-coding-challenge

## Build instruction  - contain a README.md explaining how to build and launch the API

The solution was built using Microsoft Visual Studio 2022 Community (Free) Version.
The project/application type is ASP.NET Core Web API (.NET 8). Doker desktop 4.27.2 (Linux containerization - only option on Windows Home)

A solution file (.sln) is attached to the project: powerplant-coding-challenge.sln - please double click to open the solution.
alternatively:
A project file (.csproj) has been attached to the projectpowerplant-coding-challenge.csproj - please double click to open or open project to attach.

## API Exposure on port 8888

Web API is available on port 8888.

The port configuration applies to 3 different profiles: IIS Express (http, https) and Container (Dockerfile).

IIS Express configuration: go to folder Properties\launchSettings.json => http, htpps
Containser (Dockerfile): launchUrl and ASPNETCORE_HTTPS_PORTS
    You will also see the instruction: EXPOSE 8888 in the file: Dockerfile

You should see the Debug messages in the Output section when application starts
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://[::]:8888

## Run instruction

### Https
If you have problems accessing WebAPI due to a problem related to the self-signed certificate on localhost.
In case you are using Chrome, you can go to url: chrome://flags/ and Enable option: Allow invalid certificates for resources loaded from localhost.

### Visual Studio
You can use both: Container (Dockerfile) or IIS Express profile to launch the applicaiton.
Application starts with Swagger interface exposing one POST method /ProductionPlan

## Test instruction
Launch the Web API application, you will be redirected to default swagger interface.
Expand the method ProductionPlan and click the Try it out button.
In the request body paste the content of the example payloads (\Solution Items\example_payloads): payload1.json, payload2.json, payload3.json
Click the execute button.
You shold receive Response code: 200 and the Response body with the structure of example response (\Solution Items\example_payloads\response3.json)
If you want to compare the results (numbers) with expected values (based on example payloads provided) you can use excel file (\Solution Items\example_payloads\Test_assumptions.xlsx)
The test results (visual) of the WebAPI application can be found in the Word document: (\Solution Items\example_payloads\Test_results.docx)

## Known problems & limitations
1. The system is configured to use a logger but does not implement it (e.g. Serilog) and does not produce any log.
2. The system has references to the package responsible for managing API versioning, but it has no configuration and does not use versioning (except the directory structure of the controller).
3. The algorithm used to calculate the power plant network plan is simplified in terms of calculating the merits. It takes into account the maximum score for the category.
4. For simplicity, the system uses only 1 model type. Ultimately, it should use the canonical model and map the external structure to the internal one and vice versa.
5. For simplicity, the algorithm manipulates a single structure and performs temporary calculations that are lost. 
    Ultimately, the internal structure should be extended with additional elements/attributes for the needs of a more advanced algorithm/solution.
6. For the given problem, not enough information has been provided in case we fail to complete the task (the sum of the task is too large, or after the first pass due to limitations, when some items were initially rejected, we should do a second pass)

