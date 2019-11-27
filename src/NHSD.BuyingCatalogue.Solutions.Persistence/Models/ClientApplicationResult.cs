using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class ClientApplicationResult : IClientApplicationResult
    {
        public string Id { get; set; }
        public string ClientApplication { get; set; }
    }
}
