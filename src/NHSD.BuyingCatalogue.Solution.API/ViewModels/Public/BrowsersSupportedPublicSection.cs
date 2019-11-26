using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class BrowsersSupportedPublicSection
    {
        public BrowsersSupportedPublicSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedPublicSection"/> class.
        /// </summary>
        public BrowsersSupportedPublicSection(IClientApplication clientApplication)
        {
            Answers = new BrowsersSupportedPublicSectionAnswers(clientApplication);
        }
    }
}
