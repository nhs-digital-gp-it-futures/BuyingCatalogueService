using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
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
