using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Json
{
    public class TimeSpanToTotalSecondsConverter : JsonConverter
    {
        private readonly string _format;
        public TimeSpanToTotalSecondsConverter(string format)
        {
            _format = format;
        }

        public TimeSpanToTotalSecondsConverter()
        {
            _format = "N2";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(((TimeSpan)value).TotalSeconds.ToString(_format));

            t.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false.");
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeSpan) == objectType;
        }
    }
}
