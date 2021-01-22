using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class HostingResult : IHostingResult
    {
        public string Id { get; set; }

        public string Hosting { get; set; }
    }
}
