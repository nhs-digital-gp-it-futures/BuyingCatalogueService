using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
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
