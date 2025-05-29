import requests
import json
import time

# Test payloads
payload1 = {
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
        },
        {
            "name": "gasfiredbig2",
            "type": "gasfired",
            "efficiency": 0.53,
            "pmin": 100,
            "pmax": 460
        },
        {
            "name": "gasfiredsomewhatsmaller",
            "type": "gasfired",
            "efficiency": 0.37,
            "pmin": 40,
            "pmax": 210
        },
        {
            "name": "tj1",
            "type": "turbojet",
            "efficiency": 0.3,
            "pmin": 0,
            "pmax": 16
        },
        {
            "name": "windpark1",
            "type": "windturbine",
            "efficiency": 1,
            "pmin": 0,
            "pmax": 150
        },
        {
            "name": "windpark2",
            "type": "windturbine",
            "efficiency": 1,
            "pmin": 0,
            "pmax": 36
        }
    ]
}

def test_api():
    base_url = "http://localhost:8888"
    
    print("=== Testing API ===")
    try:
        response = requests.post(
            f"{base_url}/productionplan",
            json=payload1,
            headers={"Content-Type": "application/json"}
        )
        
        print(f"Status Code: {response.status_code}")
        
        if response.status_code == 200:
            result = response.json()
            print("✅ Success! Production Plan:")
            
            total_production = 0
            for plant in result:
                print(f"  {plant['name']}: {plant['p']} MW")
                total_production += plant['p']
            
            print(f"\nTotal Production: {total_production} MW")
            print(f"Target Load: {payload1['load']} MW")
            
        else:
            print(f"❌ Error: {response.text}")
    
    except requests.exceptions.ConnectionError:
        print("❌ Cannot connect to API. Make sure the server is running!")
        print("Run: python app.py")
    except Exception as e:
        print(f"❌ Error: {str(e)}")

if __name__ == "__main__":
    test_api()