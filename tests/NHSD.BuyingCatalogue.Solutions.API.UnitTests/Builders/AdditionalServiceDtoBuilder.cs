using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetAdditionalServiceByAdditionalServiceId;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class AdditionalServiceDtoBuilder
    {
        private readonly string catalogueItemId;
        private readonly string summary;
        private readonly string catalogueItemName;
        private readonly string solutionName;
        private string solutionId;

        private AdditionalServiceDtoBuilder()
        {
            catalogueItemId = "cat";
            summary = "summary";
            catalogueItemName = "name";
            solutionId = "Sln1";
            solutionName = "sol name";
        }

        public AdditionalServiceDtoBuilder WithSolutionId(string solutionId)
        {
            this.solutionId = solutionId;
            return this;
        }

        internal static AdditionalServiceDtoBuilder Create() => new();

        internal IAdditionalService Build()
        {
            return new AdditionalServiceDto
            {
                CatalogueItemId = catalogueItemId,
                Summary = summary,
                CatalogueItemName = catalogueItemName,
                SolutionId = solutionId,
                SolutionName = solutionName,
            };
        }
    }
}
