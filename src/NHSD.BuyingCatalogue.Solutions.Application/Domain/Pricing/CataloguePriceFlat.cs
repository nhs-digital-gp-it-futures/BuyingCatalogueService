namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class CataloguePriceFlat : CataloguePriceBase
    {
        public decimal? Price { get; set; }

        public CataloguePriceFlat() : base(CataloguePriceType.Flat)
        {
        }
    }
}
