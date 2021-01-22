using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class CatalogueItemDtoBuilder
    {
        private readonly string catalogueItemId;
        private readonly string name;

        private CatalogueItemDtoBuilder()
        {
            catalogueItemId = "Some catalogue item id";
            name = "Some catalogue item name";
        }

        public static CatalogueItemDtoBuilder Create() => new();

        public CatalogueItemDto Build() => new(catalogueItemId, name);
    }
}
