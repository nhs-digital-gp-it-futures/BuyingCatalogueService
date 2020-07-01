using System;
using System.Collections.Generic;
using System.Linq;
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

            return cataloguePriceDictionary.TryGetValue(reference, out var cataloguePriceId) ? cataloguePriceId : 0;
        }

        public static int GetCataloguePriceIdsByCatalougePriceIdRef(this ScenarioContext context, string priceIdRef)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var cataloguePriceDictionary =
                context.Get<IDictionary<string,int>>(ScenarioContextKeys.CataloguePriceIdMapDictionary, new Dictionary<string,int>());
           
            return cataloguePriceDictionary.TryGetValue(priceIdRef, out var cataloguePriceId) ? cataloguePriceId : 0;
        }
    }
}
