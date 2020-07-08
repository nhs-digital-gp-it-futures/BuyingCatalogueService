using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetAdditionalServiceByAdditionalServiceId
{
    public sealed class AdditionalServiceDto : IAdditionalService
    {
        public string CatalogueItemId { get; set; }
        public string Summary { get; set; }
        public string CatalogueItemName { get; set; }
        public string SolutionId { get; set; }
        public string SolutionName { get; set; }
    }
}
