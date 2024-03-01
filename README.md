# powerplant-coding-challenge

This project is setup with SOLID principles and clean architecture. It introduces a minimal API endpoint as requested by the exercice. The methods and variable names are clear, concinse and understanding of what they do.

It also includes unit tests for a TDD approach where I was able to test the Payload 3 object and the expected payload 3 response. You can see the results for the other payloads below.


## Deployment Instructions

You can either clone this repository and build the docker image, or pull and build the docker image with the URL of the Dockerfile automatically. We could also push the image to a container registry like Docker Hub or Github Container Registry.

With repository cloned:

You can build from the home directory of the repository or CD into the Api application folder

You can then build and run the image, exposing the container port 8888 to your host's port 8888.


```

	cd ./GlobalEnergyManagement/GlobalEnergyManagement.Api/
	
	docker build -t global-energy-management-api:latest
	
	docker run -p 8888:8888 global-energy-management-api:latest

```


To pull and build I could've setup a CI/CD pipeline to build the Dockerfile and push the image to Github Container Registry. From there we could use docker pull to pull the image and build it.

## Payload 3 Result with Costs (not including CO2)

This is the result in debug mode just so you could see the costs. It wasn't requested by the exercice but I think it's important to show the costs.


[Ordered results with cost for Payload 3](/docs/ordered-results-by-cost-in-unit-test.png)


## Payload 1 Result:

```

[
  {
    "name": "windpark1",
    "p": 90
  },
  {
    "name": "windpark2",
    "p": 21.6
  },
  {
    "name": "gasfiredbig1",
    "p": 460
  },
  {
    "name": "gasfiredbig2",
    "p": 338.4
  },
  {
    "name": "gasfiredsomewhatsmaller",
    "p": 0
  },
  {
    "name": "tj1",
    "p": 0
  }
]

```

## Payload 2 Result:

```

[
  {
    "name": "windpark1",
    "p": 0
  },
  {
    "name": "windpark2",
    "p": 0
  },
  {
    "name": "gasfiredbig1",
    "p": 460
  },
  {
    "name": "gasfiredbig2",
    "p": 20
  },
  {
    "name": "gasfiredsomewhatsmaller",
    "p": 0
  },
  {
    "name": "tj1",
    "p": 0
  }
]

```