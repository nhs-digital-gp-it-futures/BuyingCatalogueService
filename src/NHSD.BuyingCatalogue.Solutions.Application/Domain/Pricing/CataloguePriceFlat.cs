namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class CataloguePriceFlat : CataloguePriceBase
    {
        public CataloguePriceFlat()
            : base(CataloguePriceType.Flat)
        {
        }

        public decimal? Price { get; set; }
    }
}
