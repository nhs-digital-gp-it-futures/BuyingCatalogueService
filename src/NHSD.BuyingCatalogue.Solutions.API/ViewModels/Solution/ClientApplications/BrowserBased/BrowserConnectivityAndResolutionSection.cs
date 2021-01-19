using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserConnectivityAndResolutionSection
    {
        public BrowserConnectivityAndResolutionSection(IClientApplication clientApplication) =>
            Answers = new BrowserConnectivityAndResolutionSectionAnswers(clientApplication);

        public BrowserConnectivityAndResolutionSectionAnswers Answers { get; }
    }
}
