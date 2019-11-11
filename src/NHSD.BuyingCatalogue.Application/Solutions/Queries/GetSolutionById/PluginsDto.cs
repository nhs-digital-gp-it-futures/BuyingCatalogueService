using NHSD.BuyingCatalogue.Contracts;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    public class PluginsDto : IPlugins
    {
        public bool? Required { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
