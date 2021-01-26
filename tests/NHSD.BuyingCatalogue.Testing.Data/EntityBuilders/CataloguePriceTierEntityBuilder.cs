using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CataloguePriceTierEntityBuilder
    {
        private readonly CataloguePriceTierEntity cataloguePriceTierEntity;

        private CataloguePriceTierEntityBuilder()
        {
            cataloguePriceTierEntity = new CataloguePriceTierEntity();
        }

        public static CataloguePriceTierEntityBuilder Create() => new();

        public CataloguePriceTierEntityBuilder WithCataloguePriceId(int id)
        {
            cataloguePriceTierEntity.CataloguePriceId = id;
            return this;
        }

        public CataloguePriceTierEntityBuilder WithBandStart(int start)
        {
            cataloguePriceTierEntity.BandStart = start;
            return this;
        }

        public CataloguePriceTierEntityBuilder WithBandEnd(int? end)
        {
            cataloguePriceTierEntity.BandEnd = end;
            return this;
        }

        public CataloguePriceTierEntityBuilder WithPrice(decimal price)
        {
            cataloguePriceTierEntity.Price = price;
            return this;
        }

        public CataloguePriceTierEntity Build()
        {
            return cataloguePriceTierEntity;
        }
    }
}
