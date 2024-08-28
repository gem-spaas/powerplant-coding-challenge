challenge description at challenge.md

# Running localy
first install dependencies
``` sh
    pip install -r requirements.txt
```


to run tests:
``` sh
    python test_main.py
```
or simply:
``` sh
    pytest
```
to run the app
``` sh
    python main.py
```

to try it send the payloads to 127.0.0.1:8888/productionplan
using the browser or a tool like postman

you can also vist 127.0.0.1:8888/docs and try the payload there

you can also check the hosted docker image here
https://extia.onrender.com/docs

# Docker
run
``` sh
    docker build --build-arg DOCKER_IP=127.0.0.1 -t powerplant_image -f Dockerfile .
    docker run -p 8888:8888 --name powerplant_container powerplant_image
```

# issues
- produced power strictly equal to load (no over production)

IRL overproduction is usually alright (in resonable ammounts)
extra power is either stored in a dam or simply sold to a connected grid
added an extra arg (overproduction) which default to false to handle this case
- kerosen produces co2 aswell, that should be accounted for too?
- co2 tax should be based on consumed energy not produced?

# Solution

generate a price range for each power plant and combine them to get the minimum cost for power

Space complexity O(N²) : price_list contains N element each may hold an array of N element (actif powerplants for that range)
Time complexity O(N log N) : looping through price_list performing a binary search (log N)