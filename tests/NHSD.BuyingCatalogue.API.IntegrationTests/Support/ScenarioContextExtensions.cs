using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Support
{
    internal static class ScenarioContextExtensions
    {
        public static TValue Get<TValue>(this ScenarioContext context, string key, TValue defaultValue) =>
            context.TryGetValue(key, out TValue value) ? value : defaultValue;

        public static int GetCataloguePriceIdByCatalougePriceTierReference(this ScenarioContext context, int reference)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var cataloguePriceDictionary =
                context.Get<IDictionary<int, int>>(ScenarioContextKeys.CatalogueTierMapDictionary, new Dictionary<int, int>());

            if (cataloguePriceDictionary.TryGetValue(reference, out var cataloguePriceId))
                return cataloguePriceId;

            return 0;
        }
    }
}
