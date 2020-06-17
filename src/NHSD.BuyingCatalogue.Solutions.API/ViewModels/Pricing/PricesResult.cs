using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Pricing
{
    public sealed class PricesResult
    {
        public int PriceId { get; set; }

        public string Type { get; set; }

        public string CurrencyCode { get; set; }

        public ItemUnitResult ItemUnit { get; set; }

        public TimeUnitResult TimeUnit { get; set; }

        public decimal? Price { get; set; }

        public int? TieringPeriod { get; set; }

        public IEnumerable<TierResult> Tiers { get; set; }
    }
}
