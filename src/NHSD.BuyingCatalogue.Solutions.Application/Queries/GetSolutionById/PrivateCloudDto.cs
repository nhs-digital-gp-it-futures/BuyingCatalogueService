using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    internal sealed class PrivateCloudDto : IPrivateCloud
    {
        public string Summary { get; set; }
        public string Link { get; set; }
        public string HostingModel { get; set; }
        public string RequiresHSCN { get; set; }
    }
}
