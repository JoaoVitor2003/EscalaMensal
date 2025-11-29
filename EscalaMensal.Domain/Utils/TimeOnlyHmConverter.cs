using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscalaMensal.Domain.Utils
{
    public class TimeOnlyHmConverter : JsonConverter<TimeOnly>
    {
        private const string Format = "HH:mm";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            if (DateTime.TryParse(value, out var dateTime))
            {
                return new TimeOnly(dateTime.Hour, dateTime.Minute);
            }

            if (TimeOnly.TryParse(value, out var timeOnly))
            {
                return new TimeOnly(timeOnly.Hour, timeOnly.Minute);
            }

            throw new JsonException($"Não foi possível converter '{value}' para TimeOnly.");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
