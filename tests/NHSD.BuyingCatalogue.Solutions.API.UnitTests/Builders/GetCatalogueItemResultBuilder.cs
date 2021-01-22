using NHSD.BuyingCatalogue.Solutions.API.ViewModels.CatalogueItems;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class GetCatalogueItemResultBuilder
    {
        private string catalogueItemId;
        private string name;

        private GetCatalogueItemResultBuilder()
        {
            catalogueItemId = "Some catalogue item id";
            name = "Some catalogue item name";
        }

        public static GetCatalogueItemResultBuilder Create() => new();

        public GetCatalogueItemResultBuilder WithCatalogueItemId(string id)
        {
            catalogueItemId = id;
            return this;
        }

        public GetCatalogueItemResultBuilder WithName(string itemName)
        {
            name = itemName;
            return this;
        }

        public GetCatalogueItemResult Build() => new()
        {
            CatalogueItemId = catalogueItemId,
            Name = name,
        };
    }
}
