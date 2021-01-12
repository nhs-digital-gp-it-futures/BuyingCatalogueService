using NHSD.BuyingCatalogue.Solutions.API.ViewModels.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class GetCatalogueItemResultBuilder
    {
        private string _catalogueItemId;
        private string _name;

        private GetCatalogueItemResultBuilder()
        {
            _catalogueItemId = "Some catalogue item id";
            _name = "Some catalogue item name";
        }

        public static GetCatalogueItemResultBuilder Create() => new();

        public GetCatalogueItemResultBuilder WithCatalogueItemId(string catalogueItemId)
        {
            _catalogueItemId = catalogueItemId;
            return this;
        }

        public GetCatalogueItemResultBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public GetCatalogueItemResult Build() =>
            new()
            {
                CatalogueItemId = _catalogueItemId,
                Name = _name,
            };
    }
}
