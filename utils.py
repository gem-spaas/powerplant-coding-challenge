from models import List, PriceMap


def find_intersection(x1, y1, x2, y2, y3, y4):
    # Compute the coefficients A1, B1, C1 for the first line
    A1 = y2 - y1
    B1 = x1 - x2
    C1 = A1 * x1 + B1 * y1

    # Compute the coefficients A2, B2, C2 for the second line (B2 == B1)
    A2 = y4 - y3
    C2 = A2 * x1 + B1 * y3

    # Compute the determinant
    D = (A1 - A2) * B1

    if D == 0:
        # The lines are parallel (no intersection)
        return None
    else:
        # Compute the x and y coordinates of the intersection point
        x = (C1 - C2) * B1 / D
        y = (A1 * C2 - A2 * C1) / D
        return (x, y)


def binary_search(pricelist: List[PriceMap], power: float) -> int:
    low = 0
    high = len(pricelist) - 1

    while low <= high:
        mid = (high + low) // 2

        # If x is greater, ignore left half
        if pricelist[mid].pmin < power:
            low = mid + 1
        # If x is smaller, ignore right half
        elif pricelist[mid].pmin > power:
            high = mid - 1
        # means x is present at mid
        else:
            return mid

    # If element was not present return lowerbound
    return low - 1


def add_ranges(old_range: PriceMap, new_range: PriceMap) -> List[PriceMap]:
    empty1 = old_range.low.copy()
    empty2 = old_range.low.copy()
    empty3 = old_range.low.copy()
    empty4 = old_range.low.copy()

    empty1.append(new_range.pid)
    empty3.append(old_range.pid)

    full1 = old_range.high.copy()
    full2 = old_range.high.copy()
    full3 = old_range.high.copy()
    full4 = old_range.high.copy()

    if new_range.cmin == new_range.cmax:
        empty2.append(new_range.pid)
    else:
        full2.append(new_range.pid)

    if old_range.cmin == old_range.cmax:
        empty4.append(old_range.pid)
    else:
        full4.append(old_range.pid)

    return [
        PriceMap(  # set the new range to minimum power and add it to the old range
            old_range.pmin + new_range.pmin, old_range.pmax + new_range.pmin,
            old_range.cmin + new_range.cmin, old_range.cmax + new_range.cmin,
            empty1,
            full1,
            old_range.pid,
        ),
        PriceMap(  # set the new range to maximum power and add it to the old range
            old_range.pmin + new_range.pmax, old_range.pmax + new_range.pmax,
            old_range.cmin + new_range.cmax, old_range.cmax + new_range.cmax,
            empty2,
            full2,
            old_range.pid,
        ),
        # do the opposite (add the new range to the old one )
        PriceMap(
            old_range.pmin + new_range.pmin, new_range.pmax + old_range.pmin,
            old_range.cmin + new_range.cmin, new_range.cmax + old_range.cmin,
            empty3,
            full3,
            new_range.pid,
        ),
        PriceMap(
            new_range.pmin + old_range.pmax, old_range.pmax + new_range.pmax,
            new_range.cmin + old_range.cmax, old_range.cmax + new_range.cmax,
            empty4,
            full4,
            new_range.pid,
        ),
    ]

def min_ranges(range1: PriceMap, range2: PriceMap) -> List[PriceMap]:
    """
    either returns 1 range (the one with lowest cost)
    if a single range cheaper
    or breaks the range in 2 if one becomes cheaper in the middle of the range
    """
    intersection = find_intersection(
        range1.pmin, range1.cmin, range1.pmax, range1.cmax, range2.cmin, range2.cmax
    )
    if not intersection or intersection[0] <= range1.pmin or  intersection[0] >= range1.pmax:
        lowest = range1 if range1.cmin < range2.cmin else range2
        return [
            PriceMap(
                lowest.pmin, lowest.pmax,
                lowest.cmin, lowest.cmax,
                lowest.low.copy(),
                lowest.high.copy(),
                lowest.pid,
            )
        ]
    elif range1.cmin < range2.cmin:
        return [
            PriceMap(
                range1.pmin, intersection[0],
                range1.cmin, intersection[1],
                range1.low.copy(),
                range1.high.copy(),
                range1.pid,
            ),
            PriceMap(
                intersection[0], range2.pmax,
                intersection[1], range2.cmax,
                range2.low.copy(),
                range2.high.copy(),
                range2.pid,
            ),
        ]
    else:
        return [
            PriceMap(
                range2.pmin, intersection[0],
                range2.cmin, intersection[1],
                range2.low.copy(),
                range2.high.copy(),
                range2.pid,
            ),
            PriceMap(
                intersection[0], range1.pmax,
                intersection[1], range1.cmax,
                range1.low.copy(),
                range1.high.copy(),
                range1.pid,
            ),
        ]


def get_overlap(range1: PriceMap, range2: PriceMap) -> tuple[List[PriceMap], PriceMap]:
    # assert range2.pmin > range1.pmin
    new_ranges = []
    if range2.pmax <= range1.pmax:  #
        if range2.pmin > range1.pmin:
            breakpoint_cost = range1.cmin + (range1.cmax - range1.cmin) * (
                range2.pmin - range1.pmin
            ) / (range1.pmax - range1.pmin)
            new_ranges.append(
                PriceMap(
                    range1.pmin, range2.pmin,
                    range1.cmin, breakpoint_cost,
                    range1.low.copy(),
                    range1.high.copy(),
                    range1.pid,
                )
            )
            range1.pmin = range2.pmin
            range1.cmin = breakpoint_cost

        old_pmax = range1.pmax
        old_cmax = range1.cmax
        range1.pmax = range2.pmax
        range1.cmax = range1.cmin + (range1.cmax - range1.cmin) * (range2.pmax - range1.pmin) / (old_pmax - range1.pmin)

        new_ranges.extend(min_ranges(range1, range2))

        if old_pmax > range2.pmax:
            range1.pmin = range1.pmax
            range1.cmin = range1.cmax
            range1.pmax = old_pmax
            range1.cmax = old_cmax
            return (new_ranges, range1)

        return (new_ranges, None)
    else:
        c1 = range1.cmin + (range1.cmax - range1.cmin) * (range2.pmin - range1.pmin) / (range1.pmax - range1.pmin)
        range1_overlap = PriceMap(
            range2.pmin, range1.pmax,
            c1, range1.cmax,
            range1.low.copy(),
            range1.high.copy(),
            range1.pid,
        )
        c2 = range2.cmin + (range2.cmax - range2.cmin) * (range1.pmax - range2.pmin) / (range2.pmax - range2.pmin)
        range2_overlap = PriceMap(
            range2.pmin, range1.pmax,
            range2.cmin, c2,
            range2.low.copy(),
            range2.high.copy(),
            range2.pid,
        )
        old_pmax = range1.pmax
        range1.pmax = range2.pmin
        range1.cmax = c1

        range2.pmin = old_pmax
        range2.cmin = c2

        if range1.pmax > range1.pmin:
            new_ranges.append(range1)
        new_ranges.extend(min_ranges(range1_overlap, range2_overlap))

        return (new_ranges, range2)


def merge_ranges(pricelist: List[PriceMap], new_ranges: List[PriceMap]) -> List[PriceMap]:
    if len(pricelist) == 0:
        return new_ranges
    for current_range in new_ranges:
        index = binary_search(pricelist, current_range.pmin)
        target = pricelist[index]

        # merge the overlaping part with the previous range
        if current_range.pmin < target.pmax and index>=0:
            pricelist.pop(index)
            overlap, extra_range = get_overlap(target, current_range)

            for min_range in overlap:
                pricelist.insert(index, min_range)
                index += 1

            if extra_range:
                if extra_range == target:
                    pricelist.insert(index, extra_range)
                    continue
                current_range = extra_range
            else:
                continue
        else:
            index += 1

        # merge the remainding part in the following ranges
        while index <= len(pricelist):
            if index == len(pricelist):
                pricelist.insert(index, current_range)
                break

            target = pricelist[index]
            if target.pmin >= current_range.pmax:
                pricelist.insert(index, current_range)
                break
            overlap, extra_range = get_overlap(current_range, target)
            if overlap:
                pricelist.pop(index)
                for min_range in overlap:
                    pricelist.insert(index, min_range)
                    index += 1
            if extra_range:
                if extra_range == target:
                    pricelist.insert(index, extra_range)
                    break
                current_range = extra_range
            else:
                break
    return pricelist
