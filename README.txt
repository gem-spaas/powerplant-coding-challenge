# Power Plant Production Plan API

A REST API that calculates optimal power production plans for multiple power plants based on load demand, fuel costs, and plant constraints.

## Quick Start

### Prerequisites
- Python 3.7+

### Installation
```bash
pip install -r requirements.txt
```

### Launch Application
```bash
python app.py
```

The API will start on `http://localhost:8888`

## API Usage

### Endpoint
**POST** `/productionplan`

### Request Example
```bash
curl -X POST http://localhost:8888/productionplan \
  -H "Content-Type: application/json" \
  -d @payload1.json
```

Or test with the provided script:
```bash
python test_api.py
```

### Request Format
```json
{
  "load": 480,
  "fuels": {
    "gas(euro/MWh)": 13.4,
    "kerosine(euro/MWh)": 50.8,
    "co2(euro/ton)": 20,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "name": "gasfiredbig1",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    }
  ]
}
```

### Response Format
```json
[
  {"name": "windpark1", "p": 90.0},
  {"name": "gasfiredbig1", "p": 390.0},
  {"name": "tj1", "p": 0.0}
]
```

## Test Cases

Three test payloads are provided:
- `payload1.json` - Normal load with 60% wind
- `payload2.json` - Normal load with 0% wind  
- `payload3.json` - High load (910 MW) with 60% wind

## Algorithm Overview

1. **Calculate costs** for each plant type:
   - Wind: 0 €/MWh
   - Gas: (fuel_cost + CO2_cost) / efficiency
   - Turbojet: fuel_cost / efficiency

2. **Sort by merit order** (cheapest first)

3. **Allocate production**:
   - Use wind power first (free)
   - Fill remaining load with conventional plants
   - Respect Pmin/Pmax constraints

## Health Check

**GET** `/health` returns `{"status": "healthy"}`

## Error Handling

- Returns appropriate HTTP status codes
- Logs all requests and errors
- Validates input payload structure

## Technical Notes

- Production values rounded to 0.1 MW precision
- CO2 emissions: 0.3 tons per MWh for gas plants
- Wind production calculated as: `pmax * wind_percentage / 100`