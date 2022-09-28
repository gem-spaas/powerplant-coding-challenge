namespace EngieApi.Processing
{
    public static class PricePerMWh
    {

        public static List<DeliverPartial> Calculate(ProductionPlanRequest request)
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
            return deliverPartials.OrderBy(i => i.Price).ThenBy(i => i.PMin).ThenBy(i => i.PMax).ToList(); ;
        }
    }
}