from energy.domain.meredit_order import MeritOrder
from energy.models.models import Payload


async def get_max_supply_by_plant(payload: Payload):
    merit = MeritOrder(payload)
    max_supply_plants = merit.calculate_merit_order()
    return max_supply_plants
