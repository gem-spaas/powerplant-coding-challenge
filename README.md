# powerplant-coding-challenge

## How to run the code

First of all, we should clone the repository by
```
git clone git@github.com:Woody1203/powerplant-coding-challenge.git
```

Then there are two cases to discuss:

### local machine

Python 3.8 and pip/pip3 need to be installed firstly and the run
```
pip install -r requirements.txt
```

If the code doesn't run well. Please also try
```
pip install -r requirements_freeze.txt
```

In the end, run
```
python api.py
```
and then the code should run correctly.

There could be four kind of response
1) the file is not provided. ("No json file provided")
2) the problem sovled successfully. ("Problem solved")
3) the problem itself is a unsolveable question("Unsolvable request")
4) other error if there is("Solver service just crashed")

For the post test, I write some simple code to verify named url_test.py by using
```
python url_test.py
```

It supposed to have the result like below(the path of the file should be changed inside of the code):

![image](https://github.com/Woody1203/powerplant-coding-challenge/blob/master/result1.png)


### virtual machine(docker)

If the docker is installed already, then we will need to create the docker group firstly by
```
sudo groupadd docker
```

Then add user to the docker group by
```
sudo usermod -aG docker $USER
```

After that, log out and log back in so that the group membership is re-evaluated
```
newgrp docker
```

In the end, run
```
docker-compose up
```
the service should be launched like this

![image](https://raw.githubusercontent.com/Woody1203/powerplant-coding-challenge/master/result2.PNG)

Actually this is my first time to use docker for real. I am a bit lost after the machine keep failing for all kinds of reason. But not it works on my side and it supposed to be the same on your side too. I cross my fingure for that.

### Analyze the ideas

This question could be regarded as a linear programming question. After bring the number into values. This work basically follow the greddy algorithm to provide an approximate solution. With my background, personally I may help on some aspect for extending this project into production on:
1) link with database for storing post results and logging information
2) predict the request of the load as a time series problem if more data provided
3) Solve this question by linear programming algorithms like simplex and maximization flow
4) try other approximate solutions like genetic algorithm
