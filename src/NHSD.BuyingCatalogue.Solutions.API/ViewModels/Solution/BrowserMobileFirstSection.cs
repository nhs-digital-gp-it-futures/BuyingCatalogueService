using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class BrowserMobileFirstSection
    {
        public BrowserMobileFirstSectionAnswers Answers { get; }

        public BrowserMobileFirstSection(IClientApplication clientApplication) =>
            Answers = new BrowserMobileFirstSectionAnswers(clientApplication);
    }
}
