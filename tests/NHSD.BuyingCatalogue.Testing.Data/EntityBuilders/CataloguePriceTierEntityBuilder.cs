using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CataloguePriceTierEntityBuilder
    {
        private readonly CataloguePriceTierEntity _cataloguePriceTierEntity;

        private CataloguePriceTierEntityBuilder()
        {
            _cataloguePriceTierEntity = new CataloguePriceTierEntity();
        }

        public static CataloguePriceTierEntityBuilder Create() => new CataloguePriceTierEntityBuilder();

        public CataloguePriceTierEntityBuilder WithCataloguePriceId(int id)
        {
            _cataloguePriceTierEntity.CataloguePriceId = id;
            return this;
        }

        public CataloguePriceTierEntityBuilder WithBandStart(int start)
        {
            _cataloguePriceTierEntity.BandStart = start;
            return this;
        }

        public CataloguePriceTierEntityBuilder WithBandEnd(int? end)
        {
            _cataloguePriceTierEntity.BandEnd = end;
            return this;
        }

        public CataloguePriceTierEntityBuilder WithPrice(decimal price)
        {
            _cataloguePriceTierEntity.Price = price;
            return this;
        }

        public CataloguePriceTierEntity Build()
        {
            return _cataloguePriceTierEntity;
        }
    }
}
