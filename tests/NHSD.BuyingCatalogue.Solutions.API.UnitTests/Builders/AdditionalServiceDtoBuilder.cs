using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetAdditionalServiceByAdditionalServiceId;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class AdditionalServiceDtoBuilder
    {
        private readonly string _catalogueItemId;
        private readonly string _summary;
        private readonly string _catalogueItemName;
        private string _solutionId;
        private readonly string _solutionName;

        private AdditionalServiceDtoBuilder()
        {
            _catalogueItemId = "cat";
            _summary = "summary";
            _catalogueItemName = "name";
            _solutionId = "Sln1";
            _solutionName = "sol name";
        }

        internal static AdditionalServiceDtoBuilder Create() => new();

        public AdditionalServiceDtoBuilder WithSolutionId(string solutionId)
        {
            _solutionId = solutionId;
            return this;
        }

        internal IAdditionalService Build()
        {
            return new AdditionalServiceDto
            {
                CatalogueItemId = _catalogueItemId,
                Summary = _summary,
                CatalogueItemName = _catalogueItemName,
                SolutionId = _solutionId,
                SolutionName = _solutionName,
            };
        }
    }
}
