# powerplant-coding-challenge

## Deploying on Docker

- You can find the Dockerfile at **cloned-repository-folder\PowerPlantCodingChallenge\PowerPlantCodingChallenge**.
- The application can be built and deployed running the commands bellow at Dockerfile's location:
```
docker build -t powerplantimage_lucascezar -f Dockerfile ..
docker run -p 8888:8888 --name powerplantcontainer_lucascezar powerplantimage_lucascezar
```
