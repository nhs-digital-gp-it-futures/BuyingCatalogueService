using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solution.API.ViewModels.Preview
{
    public class PluginOrExtensionsPreviewSection
    {
        public PluginOrExtensionsPreviewSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginOrExtensionsPreviewSection"/> class.
        /// </summary>
        public PluginOrExtensionsPreviewSection(IClientApplication clientApplication)
        {
            Answers = new PluginOrExtensionsPreviewSectionAnswers(clientApplication.Plugins);
        }
    }
}
