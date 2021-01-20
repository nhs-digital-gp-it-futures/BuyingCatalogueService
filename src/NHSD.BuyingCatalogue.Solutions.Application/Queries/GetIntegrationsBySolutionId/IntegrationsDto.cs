using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetIntegrationsBySolutionId
{
    internal sealed class IntegrationsDto : IIntegrations
    {
        public string Url { get; set; }

        public string DocumentName { get; set; }
    }
}
