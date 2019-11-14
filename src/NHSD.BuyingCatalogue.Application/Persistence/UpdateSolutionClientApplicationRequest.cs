using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class UpdateSolutionClientApplicationRequest : IUpdateSolutionClientApplicationRequest
    {
        public UpdateSolutionClientApplicationRequest(string id, ClientApplication clientApplication)
        {
            Id = id;
            ClientApplication = JsonConvert.SerializeObject(clientApplication).ToString();
        }

        public string Id { get; }

        public string ClientApplication { get; }
    }
}
