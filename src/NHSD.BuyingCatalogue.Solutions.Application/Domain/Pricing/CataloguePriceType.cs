using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class CataloguePriceType : Enumerator
    {
        public static readonly CataloguePriceType Flat = new CataloguePriceType(1, nameof(Flat));
        public static readonly CataloguePriceType Tiered = new CataloguePriceType(2, nameof(Tiered));

        public CataloguePriceType(int id, string name) : base(id, name)
        {
        }
    }
}
