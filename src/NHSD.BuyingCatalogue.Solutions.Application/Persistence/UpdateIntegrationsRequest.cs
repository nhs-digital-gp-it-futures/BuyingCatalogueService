using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateIntegrationsRequest : IUpdateIntegrationsRequest
    {
        public UpdateIntegrationsRequest(string solutionId, string url)
        {
            SolutionId = solutionId;
            Url = url;
        }

        public string SolutionId { get; }

        public string Url { get; }
    }
}
