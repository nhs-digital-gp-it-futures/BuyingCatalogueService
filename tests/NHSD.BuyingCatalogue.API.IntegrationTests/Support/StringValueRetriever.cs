using System;
using System.Collections.Generic;
using System.Globalization;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal sealed class StringValueRetriever : IValueRetriever
    {
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return propertyType == typeof(string);
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            const string nullString = "NULL";
            const string lengthString = "A string with length of ";

            var (_, value) = keyValuePair;
            if (value.Equals(nullString, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (!value.StartsWith(lengthString, StringComparison.OrdinalIgnoreCase))
                return value.Trim('"');

            var desiredStringLength = value.Replace(lengthString, string.Empty, StringComparison.InvariantCulture);

            return new string('a', int.Parse(desiredStringLength, NumberStyles.Integer, new NumberFormatInfo()));
        }
    }
}
