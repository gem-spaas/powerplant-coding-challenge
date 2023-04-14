from fastapi import FastAPI
# from src.processing import compute_loads_from_request
from dataclasses import dataclass, asdict, field
import uvicorn
import json

app = FastAPI()

@dataclass(order=True)
class PowerPlant:
    """This class represent a powerplant"""

    sort_index: int = field(init=False)
    # The type of powerplant (windturbine, gasfired, turbojet)
    ptype: str
    # The efficiency represented with a number between 0 and 1
    efficiency: float
    # The name of the powerplant
    name: str
    # The minimum amount of MWh that the powerplant can produce (before efficiency calculus)
    pmin: int
    # The maximum amount of MWh that the powerplant can produce (before efficiency calculus)
    pmax: int
    # The cost in euro to produce 1 MWh
    _cost_per_mwh: float = 0.0
    # The cost of the co2 emission based on 'p', co2 ton/MWh and price per ton
    co2_cost: float = 0.0

    # The power the powerplant has to produce to reach the load goal
    producing: float = 0

    @property
    def cost_per_mwh(self) -> float:
        return self._cost_per_mwh

    @cost_per_mwh.setter
    def cost_per_mwh(self, v: float) -> None:
        self._cost_per_mwh = v
        self.sort_index = self._cost_per_mwh


@dataclass
class Costs:
    gasfired: float
    turbojet: float
    co2: float
    # This variable is always to zero
    windturbine: float = 0



def convert_fuels_to_costs(data: dict):
    return Costs(data['gas(euro/MWh)'], data['kerosine(euro/MWh)'], data['co2(euro/ton)'])

def compute_loads_from_request(data: dict):
    goal = int(data['load'])
    costs = convert_fuels_to_costs(data['fuels'])

    powerplants = []
    for p in data['powerplants']:
        powerplant = PowerPlant(p['type'], p['efficiency'], p['name'], p['pmin'], p['pmax'])
        powerplants.append(powerplant)
        powerplant.cost_per_mwh = getattr(costs, powerplant.ptype) / powerplant.efficiency

        # The maximum power of a windturbine is limited by the wind %
        if powerplant.ptype == "windturbine":
            powerplant.pmax *= (int(data['fuels']['wind(%)']) / 100)

        # If the PowerPlant object is a wind turbine, adjust its maximum power
        # output based on the percentage of wind available.
        elif powerplant.ptype == "gasfired":
            powerplant.cost_per_mwh += costs.co2 * 0.3

    # Sort the powerplants by merit-order (here being the cost_per_mwh)
    powerplants.sort()

    reached_load = 0
    to_use = []
    for p in powerplants:
        if p.pmax == 0: continue
        if reached_load < goal:
            if reached_load + p.pmax <= goal:
                reached_load += p.pmax
                p.producing = p.pmax

            # If using the PowerPlant object's maximum power output would
            # exceed the desired load, but using its minimum power output
            # would not meet the desired load, adjust the power outputs of
            # previously used PowerPlant objects to meet the load.
            elif reached_load + p.pmin > goal:
                diff = goal - reached_load

                # Calculate the amount of power that needs to be removed
                # from previously used PowerPlant objects.
                to_remove = reached_load + p.pmin - goal

                # Iterate over the previously used PowerPlant objects in reverse order and adjust their power output as needed.
                for previous in reversed(to_use):
                    if previous.producing - to_remove > previous.pmin:
                        previous.producing -= to_remove
                        p.producing = p.pmin
                        to_remove = 0
                        break
                    else:
                        previous.producing -= (previous.pmin - to_remove)

                reached_load += diff

            elif reached_load + p.pmin < goal:
                p.producing = goal - reached_load
                reached_load = goal

            to_use.append(p)

        else:
            break

    return [{"name": p.name, "p": round(p.producing, 2)} for p in powerplants]

@app.post('/productionplant')
async def productionplant(payload: dict):
    return json.dumps(compute_loads_from_request(payload))

if __name__ == "__main__":
    # uvicorn.run(app, host="127.0.0.1", port=8888)
    uvicorn.run(app, host="0.0.0.0", port=8888)