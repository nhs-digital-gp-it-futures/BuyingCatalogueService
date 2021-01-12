using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class AdditionalServiceEntityBuilder
    {
        private string _catalogueItemId;
        private string _summary;
        private string _solutionId;

        public static AdditionalServiceEntityBuilder Create() => new();

        public AdditionalServiceEntityBuilder WithCatalogueItemId(string catalogueItemId)
        {
            _catalogueItemId = catalogueItemId;
            return this;
        }

        public AdditionalServiceEntityBuilder WithSummary(string summary)
        {
            _summary = summary;
            return this;
        }

        public AdditionalServiceEntityBuilder WithSolutionId(string solutionId)
        {
            _solutionId = solutionId;
            return this;
        }

        public AdditionalServiceEntity Build()
        {
            return new()
            {
                CatalogueItemId = _catalogueItemId,
                Summary = _summary,
                SolutionId = _solutionId,
                LastUpdated = DateTime.UtcNow,
                LastUpdatedBy = Guid.NewGuid(),
            };
        }
    }
}
