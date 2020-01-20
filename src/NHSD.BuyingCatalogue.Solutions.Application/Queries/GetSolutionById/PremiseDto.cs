using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    public sealed class PremiseDto : IOnPremise
    {
        public string Summary { get; set; }
        public string Link { get; set; }
        public string HostingModel { get; set; }
        public string RequiresHSCN { get; set; }
    }
}
