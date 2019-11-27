using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    public class StringValueRetriever : IValueRetriever
    {
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return propertyType == typeof(string);
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return keyValuePair.Value == "NULL" ? null : keyValuePair.Value;
        }
    }
}
