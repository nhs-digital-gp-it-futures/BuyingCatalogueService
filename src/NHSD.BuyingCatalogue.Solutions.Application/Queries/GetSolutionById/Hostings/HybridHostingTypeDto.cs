using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById.Hostings
{
    internal sealed class HybridHostingTypeDto : IHybridHostingType
    {
        public string Summary { get; set; }

        public string Url { get; set; }

        public string HostingModel { get; set; }

        public string ConnectivityRequired { get; set; }
    }
}
