using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById.Hostings
{
    internal sealed class PublicCloudDto : IPublicCloud
    {
        public string Summary { get; set; }
        public string URL { get; set; }
        public string ConnectivityRequired { get; set; }
    }
}
