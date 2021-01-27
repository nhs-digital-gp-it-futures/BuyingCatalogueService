using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId
{
    internal sealed class PublicCloudDto : IPublicCloud
    {
        public string Summary { get; set; }

        public string Link { get; set; }

        public string RequiresHscn { get; set; }
    }
}
