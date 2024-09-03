using MyProjectJWT.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MyProjectJWT.Converter
{
    public class GenderConverter : JsonConverter<Gender>
    {
        public override Gender Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return Enum.Parse<Gender>(value, ignoreCase: true);
        }

        public override void Write(Utf8JsonWriter writer, Gender value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
