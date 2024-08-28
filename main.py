from fastapi import FastAPI
from fastapi.responses import PlainTextResponse

from models import List, PriceMap, Req, Res
from utils import binary_search, add_ranges, merge_ranges


app = FastAPI()

@app.post("/productionplan")
async def productionplan(payload: Req) -> List[Res]:
    pricelist: List[PriceMap] = []

    for i, plant in enumerate(payload.powerplants):
        # set cost and power
        if plant.type == "gasfired":
            unit_cost = payload.fuels.gas / plant.efficiency + 0.3 * payload.fuels.co2
        elif plant.type == "turbojet":
            unit_cost = payload.fuels.kerosine / plant.efficiency
        else:  # plant.type == "windturbine"
            unit_cost = 0
            plant.pmax *= payload.fuels.wind / 100

        current_ranges: List[PriceMap] = []
        if payload.overproduction and plant.pmin > 0:
            current_ranges.append(
                PriceMap(
                    0, plant.pmin,
                    unit_cost * plant.pmin, unit_cost * plant.pmin,
                    [], [], i,
                )
            )
        if plant.pmax > 0:
            current_ranges.append(
                PriceMap(
                    plant.pmin, plant.pmax,
                    unit_cost * plant.pmin, unit_cost * plant.pmax,
                    [], [], i,
                )
            )
        new_ranges: List[PriceMap] = []
        for range1 in pricelist:
            for range2 in current_ranges:
                new_ranges.extend(add_ranges(range1, range2))

        pricelist = merge_ranges(pricelist, current_ranges)
        pricelist = merge_ranges(pricelist, new_ranges)

    if not pricelist:
        if payload.load:
            return PlainTextResponse("Not enough power", status_code=418)
        return []

    j = binary_search(pricelist, payload.load)
    if pricelist[j].pmax < payload.load or j == -1:
        if j == len(pricelist) - 1:
            return PlainTextResponse("Not enough power", status_code=418)
        else: # (no overproduction)
            return PlainTextResponse("Power production gap", status_code=418)

    res = [0] * len(payload.powerplants)
    for i in pricelist[j].high:
        res[i] = payload.powerplants[i].pmax
        payload.load -= res[i]

    for i in pricelist[j].low:
        if payload.powerplants[i].type == "windturbine":
            # since windturbines have a null unit cost
            res[i] = payload.powerplants[i].pmax
        else:
            res[i] = payload.powerplants[i].pmin
        payload.load -= res[i]

    res[pricelist[j].pid] = max(
        payload.load, payload.powerplants[pricelist[j].pid].pmin
    )

    return [
        {"name": payload.powerplants[i].name, "p": round(res[i], 2)}
        for i in range(len(payload.powerplants))
    ]


if __name__ == "__main__":
    import uvicorn

    uvicorn.run(app, host="127.0.0.1", port=8888)
