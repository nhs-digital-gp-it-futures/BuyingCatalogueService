using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class AdditionalServiceEntityBuilder
    {
        private string catalogueItemId;
        private string summary;
        private string solutionId;

        public static AdditionalServiceEntityBuilder Create() => new();

        public AdditionalServiceEntityBuilder WithCatalogueItemId(string id)
        {
            catalogueItemId = id;
            return this;
        }

        public AdditionalServiceEntityBuilder WithSummary(string text)
        {
            summary = text;
            return this;
        }

        public AdditionalServiceEntityBuilder WithSolutionId(string id)
        {
            solutionId = id;
            return this;
        }

        public AdditionalServiceEntity Build()
        {
            return new()
            {
                CatalogueItemId = catalogueItemId,
                Summary = summary,
                SolutionId = solutionId,
                LastUpdated = DateTime.UtcNow,
                LastUpdatedBy = Guid.NewGuid(),
            };
        }
    }
}
