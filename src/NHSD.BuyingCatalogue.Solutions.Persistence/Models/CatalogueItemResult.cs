using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence.CatalogueItems;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class CatalogueItemResult : ICatalogueItemResult
    {
        public string CatalogueItemId { get; set; }

        public string Name { get; set; }
    }
}
