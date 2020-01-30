using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetIntegrationsBySolutionId
{
    internal class IntegrationsDto : IIntegrations
    {
        public string Url { get; set; }
    }
}
