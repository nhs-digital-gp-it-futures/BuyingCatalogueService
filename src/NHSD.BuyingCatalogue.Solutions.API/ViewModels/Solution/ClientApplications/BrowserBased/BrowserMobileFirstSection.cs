using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserMobileFirstSection
    {
        public BrowserMobileFirstSection(IClientApplication clientApplication) =>
            Answers = new BrowserMobileFirstSectionAnswers(clientApplication);

        public BrowserMobileFirstSectionAnswers Answers { get; }
    }
}
