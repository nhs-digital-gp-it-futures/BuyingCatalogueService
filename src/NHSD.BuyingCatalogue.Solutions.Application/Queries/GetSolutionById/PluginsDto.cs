using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    public class PluginsDto : IPlugins
    {
        public bool? Required { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
