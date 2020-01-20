using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Hostings
{
    internal sealed class HybridHostingType : IHybridHostingType
    {
        public string Summary { get; set; }

        public string Url { get; set; }

        public string HostingModel { get; set; }

        public string ConnectivityRequired { get; set; }
    }
}
