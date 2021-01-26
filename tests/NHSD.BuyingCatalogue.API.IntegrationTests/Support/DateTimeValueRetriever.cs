using System;
using System.Collections.Generic;
using System.Globalization;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal sealed class DateTimeValueRetriever : IValueRetriever
    {
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return keyValuePair.Key == "LastUpdated";
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return DateTime.ParseExact(keyValuePair.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
