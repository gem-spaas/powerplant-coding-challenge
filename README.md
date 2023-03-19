![Engie PowerPlant challenge](engie.png)

## Zoltan Bak's solution for powerplant challenge

## Prerequisites:
- docker
- curl
- bash shell

The solution was only tested on ubuntu linux. 
On Windows I strongly encourage you to use latest wsl 2. the curl under the latest wsl 2 is linux compatible unlike the normal widows version of curl
Of course, docker daemon on windows need to run under Linux VM. I mean that is the safe option. 


## Building and Running the server:
in the project root directory do the following:
```shell
docker build -t poweplant_challenge_zoltan_bak .
docker run -p 8888:8888 poweplant_challenge_zoltan_bak
```
I am intentionally not providing other method to run the server then docker.

## Testing the api
in the project root directory do the following:
```shell
chmod +x execute_payloads.sh
./execute_payloads.sh
```
The paths are important as they are hardcoded in the code

## Notes

- I do not concentrate on building a multi-stage dockerfile with poetry
- I prefer working with docker images and private package repo instead of poetry, but poetry not a problem either. 2 solutions for the same problem.
- [cookiecutter](https://github.com/cookiecutter/cookiecutter)
- with fastapi I prefer clean scoping, this is why in my app is not a global variable
- I prefer run uvicorn from python, but happy to change if needed
- About types: In finance at algorithms we used float instead of decimals. My hunch it is the same here.
- For this I believe async is not needed, but probably we need to do it in production systems
- Tests - Sorry, but I leave it to the end, if time remains. I know how important those, but I believe you are more interested how well I know the other technologies that necessary for the role
- conftest.py - I added because of the pytest's weirdness
- model name aliases - I will fix them if time remains.
- Input validations - If I have time ...
- I feel a bit of contradiction about the example.json and the text of the challenge. As I cannot discuss it maybe my algorithm won't work perfectly
- I hardcoded values, but I prefer ENV variables (+python-dotenv)
- Getting tired, my naming is getting worse. but the time constraint...
- 0.1 Mw rounding - Seems a bit contradiction with the examples.
- Co2 - Sorry, no time :-(