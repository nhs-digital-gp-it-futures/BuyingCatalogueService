using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class PluginOrExtensionsSection
    {
        internal PluginOrExtensionsSection(IClientApplication clientApplication)
        {
            Answers = new PluginOrExtensionsSectionAnswers(clientApplication.Plugins);
        }

        public PluginOrExtensionsSectionAnswers Answers { get; }
    }
}
