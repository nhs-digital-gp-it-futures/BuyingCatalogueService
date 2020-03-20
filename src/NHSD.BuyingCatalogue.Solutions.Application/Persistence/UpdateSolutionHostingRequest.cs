using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateSolutionHostingRequest : IUpdateSolutionHostingRequest
    {
        public UpdateSolutionHostingRequest(string id, Hosting hosting)
        {
            SolutionId = id;
            Hosting = JsonConvert.SerializeObject(hosting, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public string SolutionId { get; }

        public string Hosting { get; }
    }
}
