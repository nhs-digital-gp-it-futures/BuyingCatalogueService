using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId
{
    internal sealed class HybridHostingTypeDto : IHybridHostingType
    {
        public string Summary { get; set; }

        public string Link { get; set; }

        public string HostingModel { get; set; }

        public string RequiresHSCN { get; set; }
    }
}
