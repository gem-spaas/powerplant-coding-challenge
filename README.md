# Hello there
My name is Abad Sethi and this is my take on the CodingChallenge by Engie.

## Introduction
In this guide you can read how to build and run the code.

## Version Requirement
- This project is created in .Net 6. To run this project locally, you need Visual studio 2022 in your machine.

## Swagger
I have used the Swagger UI to test the API and it is included in the solution.

## Steps
- Dowload this project from the Github.
- Unzip the files and open the solution file, EngieCCAbadSethi.sln using VS 2022.
- Do a Clean and Build the solution to see that everything is running smoothly.
- Run the project using IIS express, wait a few seconds and you should be able to view the swagger UI.(http://localhost:8888/swagger/index.html)
- In Swagger press on the POST tab 
- When the tap opened press on the "Try it yourself" button
- Paste an exmple payload (which is also in this repo under ..\example_payloads
- Execute and analyse the results

## Endpoint
The endpoint for this code challenge is http://localhost:8888/api/v1/productionplan, which is a POST method.
