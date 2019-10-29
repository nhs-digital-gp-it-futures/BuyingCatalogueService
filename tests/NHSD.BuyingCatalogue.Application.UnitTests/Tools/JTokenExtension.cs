using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Tools
{
    public static class JTokenExtension
    {
        public static IEnumerable<string> SelectStringValues(string fieldName, string json)
        {
            return JToken.Parse(json).SelectToken(fieldName).Select(s => s.Value<string>()).ToList();
        }
    }
}
