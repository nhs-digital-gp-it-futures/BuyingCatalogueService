using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class UpdateSolutionSummaryRequest : IUpdateSolutionSummaryRequest
    {
        public UpdateSolutionSummaryRequest(string id, string summary, string description, string aboutUrl)
        {
            SolutionId = id;
            Summary = summary;
            Description = description;
            AboutUrl = aboutUrl;
        }

        public string SolutionId { get; }

        public string Summary { get; }

        public string Description { get; }

        public string AboutUrl { get; }
    }
}
