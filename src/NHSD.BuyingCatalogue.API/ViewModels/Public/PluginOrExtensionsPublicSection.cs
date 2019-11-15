using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class PluginOrExtensionsPublicSection
    {
        public PluginOrExtensionsPublicSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginOrExtensionsPublicSection"/> class.
        /// </summary>
        public PluginOrExtensionsPublicSection(IClientApplication clientApplication)
        {
            Answers = new PluginOrExtensionsPublicSectionAnswers(clientApplication.Plugins);
        }
    }
}
