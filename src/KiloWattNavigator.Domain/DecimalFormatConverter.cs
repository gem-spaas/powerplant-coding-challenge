using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KiloWattNavigator
{
    public class DecimalFormatConverter : JsonConverter<double>
    {
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            // Format the value with two decimal places
            writer.WriteRawValue(value.ToString("0.00", CultureInfo.InvariantCulture));
        }

        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Not needed for serialization
        }
    }
}
