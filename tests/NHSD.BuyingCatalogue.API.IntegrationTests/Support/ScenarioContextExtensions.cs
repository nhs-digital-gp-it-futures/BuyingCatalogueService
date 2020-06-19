using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal static class ScenarioContextExtensions
    {
        public static TValue Get<TValue>(this ScenarioContext context, string key, TValue defaultValue) =>
            context.TryGetValue(key, out TValue value) ? value : defaultValue;

        public static int GetCataloguePriceIdByCurrencyCode(this ScenarioContext context, string currencyCode)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var cataloguePriceDictionary =
                context.Get<IDictionary<string, int>>(ScenarioContextKeys.CatalogueTierMapDictionary, new Dictionary<string, int>());

            if (cataloguePriceDictionary.TryGetValue(currencyCode, out var cataloguePriceId))
                return cataloguePriceId;

            return 0;
        }
    }
}
