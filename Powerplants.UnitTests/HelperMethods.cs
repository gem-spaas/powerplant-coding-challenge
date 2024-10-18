using BusinessLogic.Models;
using Newtonsoft.Json;

namespace PowerPlant.UnitTests
{
    internal static class HelperMethods
    {
        public static List<ProductResponse> GetResponse()
        {
            using (StreamReader reader = new StreamReader("response.json"))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<List<ProductResponse>>(jsonReader)!;

            }

        }

        public static ProductPlanInput GetPayload()
        {
            using (StreamReader reader = new StreamReader("payload.json"))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<ProductPlanInput>(jsonReader)!;

            }

        }
    }
}
