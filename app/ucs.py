import heapq
import math
from app.models import PowerPlantAsset


class Path:
    def __init__(self):
        self.powerplants = {}
        self.total_load = 0
        self.total_cost = 0
        self.explored_ids = set()
        self.enough_resource = False

    def add_power_plant(
        self, id: int, powerplant_name: str, cost: float, load: float
    ) -> None:
        self.explored_ids.add(id)
        self.powerplants[id] = {
            "powerplant_name": powerplant_name,
            "cost": cost,
            "load": load,
        }
        self.total_cost += cost
        self.total_load += load

    def __lt__(self, other):
        return self.total_cost < other.total_cost


def find_cost_ucs(
    powerplant: PowerPlantAsset, difference_load: float
) -> (float, float):
    if powerplant.min_generated_mwh < difference_load:
        if powerplant.max_generated_mwh < difference_load:
            produced_load = powerplant.max_generated_mwh
        else:
            produced_load = math.ceil(difference_load * 100) / 100
    else:
        produced_load = powerplant.min_generated_mwh
    cost = powerplant.total_cost(produced_load)
    return produced_load, cost


def uniform_cost_search(powerplants: [PowerPlantAsset], demanded_load: float) -> Path:
    len_powerplants = len(powerplants)
    paths = []
    path = Path()
    start_state = 0
    frontier = []
    produced_load, cost = find_cost_ucs(powerplants[start_state], demanded_load)
    powerplants[start_state].current_cost = cost
    powerplants[start_state].current_load = produced_load
    heapq.heappush(frontier, powerplants[start_state])
    while frontier:
        current_node = heapq.heappop(frontier)
        if current_node.id not in path.explored_ids:
            path.add_power_plant(
                current_node.id,
                current_node.name,
                current_node.current_cost,
                current_node.current_load,
            )
            # print(f"Visited node: {current_node.name}, load: {current_node.current_load}, cost: {current_node.current_cost}")
            frontier = []
        if path.total_load >= demanded_load:
            heapq.heappush(paths, path)
            path = Path()
            if start_state == len_powerplants - 1:
                break
            frontier = []
            start_state += 1
            produced_load, cost = find_cost_ucs(powerplants[start_state], demanded_load)
            heapq.heappush(frontier, powerplants[start_state])
            continue
        for powerplant in powerplants:
            if powerplant.id not in path.explored_ids:
                difference_load = demanded_load - path.total_load
                produced_load, cost = find_cost_ucs(powerplant, difference_load)
                powerplant.current_cost = cost
                powerplant.current_load = produced_load
                heapq.heappush(frontier, powerplant)
    if not paths:
        print("There is not sufficient resource")
        return path
    else:
        min_cost_path = heapq.heappop(paths)
    return min_cost_path
