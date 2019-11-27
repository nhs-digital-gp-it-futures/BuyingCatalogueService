using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateSolutionClientApplicationRequest : IUpdateSolutionClientApplicationRequest
    {
        public UpdateSolutionClientApplicationRequest(string id, ClientApplication clientApplication)
        {
            SolutionId = id;
            ClientApplication = JsonConvert.SerializeObject(clientApplication).ToString();
        }

        public string SolutionId { get; }

        public string ClientApplication { get; }
    }
}
