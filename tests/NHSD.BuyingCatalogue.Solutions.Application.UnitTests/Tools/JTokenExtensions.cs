using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools
{
    internal static class JTokenExtensions
    {
        internal static IEnumerable<string> ReadStringArray(this JToken token, string fieldName)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token.SelectToken(fieldName)?.Select(s => s.Value<string>()).ToList() ?? new List<string>(0);
        }
    }
}
