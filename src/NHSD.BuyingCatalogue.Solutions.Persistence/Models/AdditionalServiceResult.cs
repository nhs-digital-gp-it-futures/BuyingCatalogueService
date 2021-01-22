using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    public sealed class AdditionalServiceResult : IAdditionalServiceResult
    {
        public string CatalogueItemId { get; set; }

        public string CatalogueItemName { get; set; }

        public string Summary { get; set; }

        public string SolutionId { get; set; }

        public string SolutionName { get; set; }
    }
}
