using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    public sealed class PluginsDto : IPlugins
    {
        public bool? Required { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
