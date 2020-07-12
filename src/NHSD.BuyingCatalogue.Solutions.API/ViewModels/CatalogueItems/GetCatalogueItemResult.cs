using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.CatalogueItems
{
    public sealed class GetCatalogueItemResult
    {
        public string CatalogueItemId { get; set; }

        public string Name { get; set; }

        public GetCatalogueItemResult()
        {
        }
    }
}
