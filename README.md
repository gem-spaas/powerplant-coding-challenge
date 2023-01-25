# Powerplant coding challenge: Jose Gutierrez

## How to deploy the API

This app uses *Docker*, so **cd** into **/src** folder and execute:

```
docker build -t myimage .
```

```
docker run -d --name mycontainer -p 8888:8888 myimage
```

Now, you can POST to 
````
0.0.0.0:8888/productionplan
````
including a body as in example_payloads folder, and the response will be like *example_response.json*.

#HappyCoding!