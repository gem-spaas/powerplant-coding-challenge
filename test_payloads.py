import json
from app import process_input

import pytest


def test_payload_1():
    expected = [
        {
            "name": "windpark1",
            "p": 150
        },
        {
            "name": "windpark2",
            "p": 36
        },
        {
            "name": "gasfiredbig1",
            "p": 294
        }
    ]
    with open('example_payloads/payload1.json') as json_file:
        data = json.load(json_file)
        result = process_input(data)
        sum_expected = [e['p'] for e in expected]

        assert result == expected
        assert sum(sum_expected) == data.get("load")


def test_payload_2():
    expected = [
        {
            "name": "gasfiredbig1",
            "p": 240
        },
        {
            "name": "gasfiredbig2",
            "p": 240
        }
    ]
    with open('example_payloads/payload2.json') as json_file:
        data = json.load(json_file)
        result = process_input(data)
        sum_expected = [e['p'] for e in expected]
        assert result == expected
        assert sum(sum_expected) == data.get("load")


def test_payload_3():
    expected = [
        {
            "name": "windpark1",
            "p": 150
        },
        {
            "name": "windpark2",
            "p": 36
        },
        {
            "name": "gasfiredbig1",
            "p": 362
        },
        {
            "name": "gasfiredbig2",
            "p": 362
        }
    ]
    with open('example_payloads/payload3.json') as json_file:
        data = json.load(json_file)
        result = process_input(data)
        sum_expected = [e['p'] for e in expected]
        assert result == expected
        assert sum(sum_expected) == data.get("load")


def test_payload_4():
    expected = [
        {
            "name": "windpark1",
            "p": 150
        },
        {
            "name": "windpark2",
            "p": 36
        },
        {
            "name": "gasfiredbig1",
            "p": 460
        },
        {
            "name": "gasfiredbig2",
            "p": 460
        },
        {
            "name": "gasfiredsomewhatsmaller",
            "p": 194
        }
    ]
    with open('example_payloads/payload4.json') as json_file:
        data = json.load(json_file)
        result = process_input(data)
        sum_expected = [e['p'] for e in expected]
        assert result == expected
        assert sum(sum_expected) == data.get("load")
