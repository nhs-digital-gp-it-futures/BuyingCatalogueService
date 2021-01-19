using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowsersSupportedSection
    {
        public BrowsersSupportedSection(IClientApplication clientApplication)
        {
            Answers = new BrowsersSupportedSectionAnswers(clientApplication);
        }

        public BrowsersSupportedSectionAnswers Answers { get; }
    }
}
