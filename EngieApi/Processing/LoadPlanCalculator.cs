namespace EngieApi.Processing;

public class LoadPlanCalculator: ILoadPlanCalculator
{
    public ProductionPlanResponse? GetLoadPlan(ProductionPlanRequest request)
    {
        ProductionPlanResponse response = new ProductionPlanResponse();
        response.ProductionPlans = new List<ProductionPlan>();
        int load = 0;
        var deliverPartials = CostCalculator.Calculate(request);
        for (int i = 0; i < deliverPartials.Count(); i++)
        {
            var pMin = deliverPartials[i].PMin;
            var pMax = deliverPartials[i].PMax;
            var power = 0;
            switch (GetUsageCondition(request.Load, load, pMax, pMin))
            {
                case PowerPlantUsage.NotUsed:
                    break;
                case PowerPlantUsage.MaximumPossibleUsed:
                    power = CalcPMax(deliverPartials, i, request.Load - load);
                    break;
                case PowerPlantUsage.PartiallyUsed:
                    power = request.Load - load;
                    break;
                case PowerPlantUsage.MinimumUsed:
                    power = pMin;
                    break;
            }
            response.ProductionPlans.Add(new ProductionPlan { Name = deliverPartials[i].Name, P = power });
            load += power;
        }
        return load != request.Load ? null : response;
    }

    private static PowerPlantUsage GetUsageCondition(int requestLoad, int resultLoad, int pMax, int pMin)
    {
        if (requestLoad == resultLoad) return PowerPlantUsage.NotUsed;
        if (pMax > 0 && requestLoad >= pMax + resultLoad) return PowerPlantUsage.MaximumPossibleUsed;
        if (requestLoad < pMax + resultLoad && pMin < requestLoad - resultLoad) return PowerPlantUsage.PartiallyUsed;
        if (requestLoad >= pMin + resultLoad) return PowerPlantUsage.MinimumUsed;

        return 0;
    }

    private static int CalcPMax(List<DeliverPartial> deliverPartials, int iStart, int rest)
    {
        for (int j = deliverPartials[iStart].PMax; j > deliverPartials[iStart].PMin; j--)
        {
            int sumMin = 0;
            int sumMax = 0;
            for (int i = iStart + 1; i < deliverPartials.Count(); i++)
            {
                if (deliverPartials[iStart].PMin == 0 || deliverPartials[iStart + 1].PMin < rest - j)
                {
                    sumMax += deliverPartials[i].PMax;
                    if (rest <= sumMax + j) return j;
                }
                else
                {
                    sumMin += deliverPartials[i].PMin;
                    if (rest == sumMin + j) return j;
                }
            }
        }
        if (deliverPartials[iStart].PMax == rest || deliverPartials[iStart].PMax < rest) return deliverPartials[iStart].PMax;
        return 0;
    }
}

public enum PowerPlantUsage
{
    NotUsed = 0,
    MaximumPossibleUsed = 1,
    PartiallyUsed = 2,
    MinimumUsed = 3
}

