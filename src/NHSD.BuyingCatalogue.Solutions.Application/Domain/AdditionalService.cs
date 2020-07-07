using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    public sealed class AdditionalService
    {
        public string CatalogueItemId { get; set; }

        public string Summary { get; set; }

        public string CatalogueItemName { get; set; }

        public string SolutionId { get; set; }

        public string SolutionName { get; set; }

        public AdditionalService(IAdditionalServiceResult additionalServiceResult)
        {
            if (additionalServiceResult is null)
                throw new ArgumentNullException(nameof(additionalServiceResult));

            CatalogueItemId = additionalServiceResult.CatalogueItemId;
            Summary = additionalServiceResult.Summary;
            CatalogueItemName = additionalServiceResult.CatalogueItemName;
            SolutionId = additionalServiceResult.SolutionId;
            SolutionName = additionalServiceResult.SolutionName;
        }
    }
}
