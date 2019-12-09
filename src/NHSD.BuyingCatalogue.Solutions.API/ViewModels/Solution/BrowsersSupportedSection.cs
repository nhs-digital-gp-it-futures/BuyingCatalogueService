using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class BrowsersSupportedSection
    {
        public BrowsersSupportedSectionAnswers Answers { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedSection"/> class.
        /// </summary>
        public BrowsersSupportedSection(IClientApplication clientApplication)
        {
            Answers = new BrowsersSupportedSectionAnswers(clientApplication);
        }
    }
}
