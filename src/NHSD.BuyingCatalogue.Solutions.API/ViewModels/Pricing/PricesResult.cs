using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing
{
    public sealed class PricesResult
    {
        public string Type { get; set; }

        public string CurrencyCode { get; set; }

        public ItemUnitResult ItemUnit { get; set; }

        public TimeUnitResult TimeUnit { get; set; }

        public decimal? Price { get; set; }

        public int? TieringPeriod { get; set; }

        public IEnumerable<TierResult> Tiers { get; set; }
    }
}
