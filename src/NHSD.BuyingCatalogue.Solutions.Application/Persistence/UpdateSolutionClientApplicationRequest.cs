using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateSolutionClientApplicationRequest : IUpdateSolutionClientApplicationRequest
    {
        public UpdateSolutionClientApplicationRequest(string id, ClientApplication clientApplication)
        {
            SolutionId = id;
            ClientApplication = JsonConvert.SerializeObject(clientApplication);
        }

        public string SolutionId { get; }

        public string ClientApplication { get; }
    }
}
