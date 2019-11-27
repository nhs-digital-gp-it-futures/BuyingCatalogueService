using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
{
    public class BrowsersSupportedPreviewSection
    {
        public BrowsersSupportedPreviewSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedPreviewSection"/> class.
        /// </summary>
        public BrowsersSupportedPreviewSection(IClientApplication clientApplication)
        {
            Answers = new BrowsersSupportedPreviewSectionAnswers(clientApplication);
        }
    }
}
