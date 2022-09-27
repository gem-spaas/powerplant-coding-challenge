namespace EngieApi.Processing;

public static class Calculator
{
    public static List<ProductionPlanResponse> GetLoadPlan(ProductionPlanRequest request)
    {
        List<ProductionPlanResponse> response = new List<ProductionPlanResponse>();
        List<DeliverPartial> deliverPartials = PriceCalc(request);
        PrepareResponse(request, response, deliverPartials);
        return response;
    }

    private static void PrepareResponse(
        ProductionPlanRequest request,
        List<ProductionPlanResponse> response,
        List<DeliverPartial> deliverPartialsUnsort)
    {
        int load = 0;
        var deliverPartials = deliverPartialsUnsort.OrderBy(i => i.Price).ThenBy(i => i.PMin).ThenBy(i => i.PMax).ToList();
        for (int i = 0; i < deliverPartials.Count(); i++)
        {
            var deliverPartial = deliverPartials[i];
            if (request.Load == load)
            {
                response.Add(new ProductionPlanResponse
                {
                    Name = deliverPartial.Name,
                    P = 0
                });
                continue;
            }
            if (deliverPartial.PMax > 0 && request.Load >= deliverPartial.PMax + load)
            {
                int pMax = CalcPMax(deliverPartials, i, request.Load - load);
                response.Add(new ProductionPlanResponse
                {
                    Name = deliverPartial.Name,
                    P = pMax
                });
                load += pMax;
                continue;
            }
            if (request.Load < deliverPartial.PMax + load && deliverPartial.PMin < request.Load - load)
            {
                response.Add(new ProductionPlanResponse
                {
                    Name = deliverPartial.Name,
                    P = request.Load - load
                });
                load = request.Load;
                continue;
            }
            if (request.Load >= deliverPartial.PMin + load)
            {
                response.Add(new ProductionPlanResponse
                {
                    Name = deliverPartial.Name,
                    P = deliverPartial.PMin
                });
                load += deliverPartial.PMin;
                continue;
            }
        }
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

    private static List<DeliverPartial> PriceCalc(ProductionPlanRequest request)
    {
        var deliverPartials = new List<DeliverPartial>();
        foreach (PowerPlant powerPlant in request.PowerPlants)
        {
            DeliverPartial deliverPartial = new DeliverPartial
            {
                Name = powerPlant.Name,
                PMax = powerPlant.PMax,
                PMin = powerPlant.PMin,
            };
            switch (powerPlant.Type)
            {
                case "gasfired":
                    deliverPartial.Price = request.Fuels.Gas / powerPlant.Efficiency + request.Fuels.Co2 * (decimal)0.3;
                    break;
                case "turbojet":
                    deliverPartial.Price = request.Fuels.Kerosine / powerPlant.Efficiency;
                    break;
                case "windturbine":
                    deliverPartial.Price = 0;
                    deliverPartial.PMax = (int)(deliverPartial.PMax * request.Fuels.Wind / 100);
                    break;
                default:
                    throw new Exception("This type of power plant doesn't exist");
            }
            deliverPartials.Add(deliverPartial);

        }
        return deliverPartials;
    }
}
