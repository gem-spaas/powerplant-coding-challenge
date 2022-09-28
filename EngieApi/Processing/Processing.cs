namespace EngieApi.Processing;

public static class Calculator
{
    public static ProductionPlanResponse GetLoadPlan(ProductionPlanRequest request)
    {
        ProductionPlanResponse response = new ProductionPlanResponse();
        response.ProductionPlans = new List<ProductionPlan>();
        int load = 0;
        var deliverPartials = PricePerMWh.Calculate(request);
        for (int i = 0; i < deliverPartials.Count(); i++)
        {
            var name = deliverPartials[i].Name;
            var pMin = deliverPartials[i].PMin;
            var pMax = deliverPartials[i].PMax;
            var power = 0;
            switch (GetCondition(request.Load, load, pMax, pMin))
            {
                case PowerPlantUsage.NotUsed:
                    break;
                case PowerPlantUsage.MaximUsed:
                    power = CalcPMax(deliverPartials, i, request.Load - load);
                    break;
                case PowerPlantUsage.PartiallyUsed:
                    power = request.Load - load;
                    break;
                case PowerPlantUsage.MinimUsed:
                    power = pMin;
                    break;
            }
            response.ProductionPlans.Add(new ProductionPlan { Name = name, P = power });
            load += power;
        }
        return response;
    }

    private static PowerPlantUsage GetCondition(int requestLoad, int resultLoad, int pMax, int pMin)
    {
        if (requestLoad == resultLoad) return PowerPlantUsage.NotUsed;
        if (pMax > 0 && requestLoad >= pMax + resultLoad) return PowerPlantUsage.MaximUsed;
        if (requestLoad < pMax + resultLoad && pMin < requestLoad - resultLoad) return PowerPlantUsage.PartiallyUsed;
        if (requestLoad >= pMin + resultLoad) return PowerPlantUsage.MinimUsed;

        return 0;
    }

    private static int CalcPMax(List<DeliverPartial> deliverPartials, int iStart, int rest)
    {
        for (int j = deliverPartials[iStart].PMax; j > 0; j--)
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
    MaximUsed = 1,
    PartiallyUsed = 2,
    MinimUsed = 3
}

