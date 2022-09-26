# powerplant-coding-challenge

If you want to follow this step-by-step. Please install some tools that will be required here.

.NET 6: https://dotnet.microsoft.com/download/dotnet/6.0
Docker: https://docs.docker.com/get-docker/

## Build the Image and Run the Container

- Build your image use ```docker build . -t engieapi```
- Run a container with previous image. ```docker run --name engieapi -p 8888:80 -d engieapi```
- Check your container. ```docker ps```. You would see like this.

## Test 

### Run test with Postman

- To send a request to an existing API, click the new button in the top-left corner of the Postman interface.
- The top-left corner of the pop-up window has an option to create a basic HTTP request.
- There are several important fields in the request interface. The first field you should attend to is the request method (or verb) field. The default request method is GET, but you need to select `POST`.
- The URL field is directly beside the request method field. To test an API, you simply select the request method and provide the appropriate URL http://localhost:8888/productionplan.
- Open the Body tab, select the `RAW` option and enter the payload  in the window below. Payloads can be found in the example_payloads folder.
- Postman will return a json as in example_response.json.

### Run test with CURL

- Open the terminal.
- Enter the following command: ```curl -X POST --data @example_payloads/payload1.json -H 'Content-Type: application/json' -H 'Accept: /' http://localhost:8888/productionplan```. At the `data` option you can change the payloads