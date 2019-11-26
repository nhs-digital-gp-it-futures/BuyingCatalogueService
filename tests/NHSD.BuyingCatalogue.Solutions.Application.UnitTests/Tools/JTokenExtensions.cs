using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools
{
    public static class JTokenExtensions
    {
        public static IEnumerable<string> ReadStringArray(this JToken token, string fieldName)
        {
            return token.SelectToken(fieldName).Select(s => s.Value<string>()).ToList();
        }
    }
}
