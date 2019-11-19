using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Persistence.Models
{
    internal sealed class ClientApplicationResult : IClientApplicationResult
    {
        public string Id { get; set; }
        public string ClientApplication { get; set; }
    }
}
