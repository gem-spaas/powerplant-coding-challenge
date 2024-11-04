# powerplant-coding-challenge - Candidate: Alberto Morales Morales


## Set-up

Set the repo base path
REPO_BASE_PATH=...
export REPO_BASE_PATH

### with conda

conda create -n ppcch python=3.10
conda activate ppcch

### without conda

### in both cases
cd $REPO_BASE_PATH/api/code
pip install -r requirements.txt

## How to run
cd $REPO_BASE_PATH/api/code
python3 run_server.py

### with Docker
cd $REPO_BASE_PATH/api
docker build -t powerplanner . 
docker run -p 8888:8888 docker.io/library/powerplanner