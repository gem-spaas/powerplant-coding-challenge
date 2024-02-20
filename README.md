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

## Solution quality
Before the solution (algorithm) was coded using C#, the results were converted using an alternative method (excel). 
The expected results and the logic accompanying the solution using the 3 examples provided are located in the file \Solutin Items\Test_assumptions.xlsx

The interface can be tested in several ways. The solution starts with a redirection to the Open API graphical interface (swagger). 
It is also possible to test using an external tool such as Postman (the Postman project is not included). 
For convenience, application test results using 3 examples are provided in Solution Items\Test_results.docx

The application compiles without any errors or warnings (all resolved).

The application has a separate project containing unit tests (Nunit), for simplicity it is not included in the solution. 
The concept and results can be seen in Solution Items\Unit_tests.docx

## Run instruction

### Https
If you have problems accessing WebAPI due to a problem related to the self-signed certificate on localhost.
In case you are using Chrome, you can go to url: chrome://flags/ and Enable option: Allow invalid certificates for resources loaded from localhost.
If you testing from IIS Express / https profile you can click continue unsecured option

### Visual Studio
You can use both: Container (Dockerfile) or IIS Express profile to launch the applicaiton.
Application starts with Swagger interface exposing one POST method /ProductionPlan

## Test instruction
1. Launch the Web API application, you will be redirected to default swagger interface.
2. Expand the method ProductionPlan and click the Try it out button.
3. In the request body paste the content of the example payloads (\Solution Items\example_payloads): payload1.json, payload2.json, payload3.json
4. Click the execute button.
5. You shold receive Response code: 200 and the Response body with the structure of example response (\Solution Items\example_payloads\response3.json)
6. If you want to compare the results (numbers) with expected values (based on example payloads provided) you can use excel file (\Solution Items\example_payloads\Test_assumptions.xlsx)
7. The test results (visual) of the WebAPI application can be found in the Word document: (\Solution Items\example_payloads\Test_results.docx)

## Known problems & limitations
1. The system is configured to use a logger but does not implement it (e.g. Serilog) and does not produce any log.
2. The system has references to the package responsible for managing API versioning, but it has no configuration and does not use versioning (except the directory structure of the controller).
3. The algorithm used to calculate the power plant network plan is simplified in terms of calculating the merits. It takes into account the maximum score for the category.
4. For simplicity, the system uses only 1 model type. Ultimately, it should use the canonical model and map the external structure to the internal one and vice versa.
5. For simplicity, the algorithm manipulates a single structure and performs temporary calculations that are lost. 
    Ultimately, the internal structure should be extended with additional elements/attributes for the needs of a more advanced algorithm/solution.
6. For the given problem, not enough information has been provided in case we fail to complete the task (the sum of the task is too large, or after the first pass due to limitations, 
    when some items were initially rejected, we should do a second pass)
7. For simplicity purpose (but mainly due to lack of necessary information), the algorithm does not take into account the effective minimum power level of the plant. 
    There is no information on what the next step should be in this case, whether to abandon the allocation altogether or to make the allocation during the next iteration, 
    when the power level has not been met by the remaining power plants on the list.
8. Error handling is simplified, errors and exceptions are propagated to the controller, which returns the 500 Internal Server Error code for unaddressed problems.
9. Lack of a system notification when the demand for energy is greater than the capacity to produce it.
10. Validation of incoming and outgoing data is very simplified (it should be done as early as possible, i.e. at the schema level). No information about metadata (requirement, etc.)
