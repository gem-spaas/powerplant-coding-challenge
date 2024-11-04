import os

import setuptools

with open("README.md", "r") as fh:
    long_description = fh.read()

# populate install_requires from requirements.txt file
thelibFolder = os.path.dirname(os.path.realpath(__file__))
requirementPath = thelibFolder + "/requirements.txt"
real_deps = []
if os.path.isfile(requirementPath):
    with open(requirementPath) as f:
        real_deps = f.read().splitlines()

# populate tests_require from test-requirements.txt file
thelibFolder = os.path.dirname(os.path.realpath(__file__))
requirementPath = thelibFolder + "/requirements.txt"
tests_deps = []
if os.path.isfile(requirementPath):
    with open(requirementPath) as f:
        tests_deps = f.read().splitlines()

setuptools.setup(
    name="powerplant-coding-challenge",
    version="1.0.0",
    author="Alberto Morales Morales",
    author_email="data_devops@grpr.com",
    description="Powerplant-coding-challenge API",
    long_description=long_description,
    long_description_content_type="text/markdown",
    url="https://github.com/gems-st-ib/powerplant-coding-challenge.git",
    packages=setuptools.find_packages(exclude=["tests"]),
    package_data={
        # If any package contains *.txt files, include them:
        "": ["*.json"]
    },
    classifiers=[
        "Programming Language :: Python :: 3",
        "License :: OSI Approved :: MIT License",
        "Operating System :: OS Independent",
    ],
    install_requires=real_deps,
    extras_require={"testing": tests_deps},
    tests_require=tests_deps,
)
