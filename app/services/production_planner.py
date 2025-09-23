from dataclasses import dataclass
from typing import List, Literal

from app.schemas.payload import ProductionPlanRequest


@dataclass
class Dispatchable:
    name: str
    kind: Literal["windturbine", "gasfired", "turbojet"]
    marginal_cost: float  # euro per MWh
    pmin: float
    pmax: float
    p: float = 0.0

    @property
    def headroom(self) -> float:
        return max(0.0, self.pmax - self.p)

    @property
    def reducible(self) -> float:
        # how much we can reduce while staying above pmin
        return max(0.0, self.p - self.pmin)


CO2_TON_PER_MWH = 0.3  # only for gas-fired according to challenge extras


def plan_production(req: ProductionPlanRequest) -> list[Dispatchable]:
    load = float(req.load)
    if load == 0:
        return [
            Dispatchable(pp.name, pp.type, 0.0, 0.0, 0.0, 0.0) for pp in req.powerplants
        ]

    # Build dispatchable units with effective costs and capacities
    items: list[Dispatchable] = []
    wind_factor = max(0.0, min(100.0, float(req.fuels.wind))) / 100.0

    for pp in req.powerplants:
        if pp.type == "windturbine":
            pmax_eff = pp.pmax * wind_factor
            items.append(
                Dispatchable(
                    name=pp.name,
                    kind=pp.type,
                    marginal_cost=0.0,
                    pmin=0.0,
                    pmax=pmax_eff,
                )
            )
        elif pp.type == "gasfired":
            # cost per MWh produced
            fuel_cost = req.fuels.gas / pp.efficiency
            co2_cost = CO2_TON_PER_MWH * req.fuels.co2
            items.append(
                Dispatchable(
                    name=pp.name,
                    kind=pp.type,
                    marginal_cost=fuel_cost + co2_cost,
                    pmin=pp.pmin,
                    pmax=pp.pmax,
                )
            )
        elif pp.type == "turbojet":
            fuel_cost = req.fuels.kerosine / pp.efficiency
            items.append(
                Dispatchable(
                    name=pp.name,
                    kind=pp.type,
                    marginal_cost=fuel_cost,
                    pmin=pp.pmin,
                    pmax=pp.pmax,
                )
            )
        else:
            raise ValueError(f"Unsupported powerplant type: {pp.type}")

    # Sort by marginal cost asc; for equal cost, larger pmax first then better efficiency effect not tracked, fall back to name
    items.sort(key=lambda d: (d.marginal_cost, -d.pmax, d.name))

    remaining = load

    # First, fully utilize wind (cheapest, pmin 0)
    for d in items:
        if d.kind == "windturbine" and remaining > 0:
            take = min(d.pmax, remaining)
            d.p = take
            remaining -= take

    # Then thermal units by merit order
    committed: List[Dispatchable] = [
        d for d in items if d.p > 0 or d.kind == "windturbine"
    ]

    for d in items:
        if d.kind == "windturbine":
            # already handled
            continue
        if remaining <= 0:
            break

        # Try to fill remaining using already committed units (headroom) before starting a new one with pmin
        if committed:
            remaining = _increase_existing_to_fill(remaining, committed)
            if remaining <= 0:
                break

        # Need this unit
        if remaining >= d.pmin:
            take = min(d.pmax, remaining)
            d.p = take
            remaining -= take
            committed.append(d)
        else:
            # remaining < pmin: try not to start this unit by increasing previous headroom
            if committed:
                rem2 = _increase_existing_to_fill(remaining, committed)
                if rem2 <= 0:
                    remaining = 0.0
                    break
            # Otherwise, we must start this unit at pmin and reduce some previous output to keep balance
            d.p = d.pmin
            committed.append(d)
            # We overshoot by (pmin - remaining)
            overflow = d.pmin - remaining
            reduced = _reduce_previous_to_absorb(overflow, committed[:-1])
            if reduced + 1e-9 < overflow:
                # Could not absorb overflow -> infeasible
                raise ValueError(
                    "Infeasible dispatch with given pmin/pmax constraints to match load exactly"
                )
            remaining = 0.0

    # After loop, if still remaining, try to increase existing including the most expensive if any left
    if remaining > 0:
        remaining = _increase_existing_to_fill(
            remaining, [d for d in items if d.kind != "windturbine"]
        )  # include all thermals
        if remaining > 1e-6:
            raise ValueError("Insufficient capacity to meet the load")

    # Rounding to 0.1 and adjustment
    _round_and_adjust(items, load)

    # Return in original input order as typical expected response lists all plants
    name_to_item = {d.name: d for d in items}
    ordered = [name_to_item[pp.name] for pp in req.powerplants]
    return ordered


def _increase_existing_to_fill(remaining: float, units: List[Dispatchable]) -> float:
    if remaining <= 0:
        return 0.0
    for u in units:
        if remaining <= 0:
            break
        head = u.headroom
        if head <= 0:
            continue
        inc = min(head, remaining)
        u.p += inc
        remaining -= inc
    return remaining


def _reduce_previous_to_absorb(overflow: float, units: List[Dispatchable]) -> float:
    reduced_total = 0.0
    # Reduce from most expensive first to keep merit order optimal
    for u in sorted(units, key=lambda x: (-x.marginal_cost, x.pmin)):
        if reduced_total >= overflow - 1e-9:
            break
        reducible = u.reducible
        if reducible <= 0:
            continue
        take = min(reducible, overflow - reduced_total)
        u.p -= take
        reduced_total += take
    return reduced_total


def _round_and_adjust(items: List[Dispatchable], load: float):
    # Round to 0.1 MW
    for d in items:
        d.p = round(d.p * 10) / 10.0
        # Clip within bounds after rounding
        if d.kind == "windturbine":
            d.p = max(0.0, min(d.p, d.pmax))
        else:
            d.p = max(d.pmin, min(d.p, d.pmax))

    target = round(load * 10) / 10.0
    current = sum(d.p for d in items)
    diff = round((target - current) * 10) / 10.0
    if abs(diff) < 0.05:
        return

    step = 0.1 if diff > 0 else -0.1
    # For increasing, prefer cheapest with headroom; for decreasing, prefer most expensive with reducible
    for _ in range(5000):
        if abs(diff) < 0.05:
            break
        if step > 0:
            # try units with headroom
            candidates = sorted(items, key=lambda x: (x.marginal_cost, -x.pmax, x.name))
            changed = False
            for u in candidates:
                max_allow = u.pmax if u.kind != "windturbine" else u.pmax
                if u.p + 1e-9 <= max_allow - 0.099:
                    u.p = round((u.p + 0.1) * 10) / 10.0
                    diff = round((diff - 0.1) * 10) / 10.0
                    changed = True
                    break
            if not changed:
                raise ValueError(
                    "Cannot adjust up to reach exact load at 0.1 MW granularity"
                )
        else:
            candidates = sorted(items, key=lambda x: (-x.marginal_cost, x.pmin))
            changed = False
            for u in candidates:
                min_allow = u.pmin if u.kind != "windturbine" else 0.0
                if u.p >= min_allow + 0.099:
                    u.p = round((u.p - 0.1) * 10) / 10.0
                    diff = round((diff + 0.1) * 10) / 10.0
                    changed = True
                    break
            if not changed:
                raise ValueError(
                    "Cannot adjust down to reach exact load at 0.1 MW granularity"
                )
