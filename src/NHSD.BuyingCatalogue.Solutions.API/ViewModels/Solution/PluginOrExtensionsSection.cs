using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class PluginOrExtensionsSection
    {
        public PluginOrExtensionsSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginOrExtensionsSection"/> class.
        /// </summary>
        public PluginOrExtensionsSection(IClientApplication clientApplication)
        {
            Answers = new PluginOrExtensionsSectionAnswers(clientApplication.Plugins);
        }
    }
}
