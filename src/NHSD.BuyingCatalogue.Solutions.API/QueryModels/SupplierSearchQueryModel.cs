using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.QueryModels
{
    public sealed class SupplierSearchQueryModel
    {
        public string Name { get; set; }

        public PublishedStatus? SolutionPublicationStatus { get; set; }

        public CatalogueItemType? CatalogueItemType { get; set; }
    }
}
