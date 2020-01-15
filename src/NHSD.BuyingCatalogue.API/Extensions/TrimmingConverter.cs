using System;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.Extensions
{
    public class TrimmingConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            return ((string)reader?.Value)?.Trim();
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            // as CanWrite is set to false, this function will never get called.
            // Implementation is required though, as this is an abstract function
            throw new NotImplementedException();
        }
    }
}
