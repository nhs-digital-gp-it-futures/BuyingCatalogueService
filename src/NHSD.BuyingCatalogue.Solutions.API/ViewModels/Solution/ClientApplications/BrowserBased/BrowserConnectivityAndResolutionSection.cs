using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public class BrowserConnectivityAndResolutionSection
    {
        public BrowserConnectivityAndResolutionSectionAnswers Answers { get; }

        public BrowserConnectivityAndResolutionSection(IClientApplication clientApplication) =>
            Answers = new BrowserConnectivityAndResolutionSectionAnswers(clientApplication);
    }
}
