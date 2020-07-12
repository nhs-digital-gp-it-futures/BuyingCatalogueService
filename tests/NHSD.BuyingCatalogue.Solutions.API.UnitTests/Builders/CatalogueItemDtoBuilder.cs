using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class CatalogueItemDtoBuilder
    {
        private string _catalogueItemId;
        private string _name;

        private CatalogueItemDtoBuilder()
        {
            _catalogueItemId = "Some catalogue item id";
            _name = "Some catalogue item name";
        }

        public static CatalogueItemDtoBuilder Create() => new CatalogueItemDtoBuilder();

        public CatalogueItemDtoBuilder WithCatalogueItemId(string catalogueItemId)
        {
            _catalogueItemId = catalogueItemId;
            return this;
        }

        public CatalogueItemDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CatalogueItemDto Build() => new CatalogueItemDto(_catalogueItemId, _name);
    }
}
