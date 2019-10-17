using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class UpdateSolutionSummaryRequest : IUpdateSolutionSummaryRequest
    {
        public string Id { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string AboutUrl { get; set; }
    }
}
