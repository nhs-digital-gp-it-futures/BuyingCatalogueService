using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing
{
    public sealed class PricingResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PricesResult> Prices { get; set; }
    }
}
