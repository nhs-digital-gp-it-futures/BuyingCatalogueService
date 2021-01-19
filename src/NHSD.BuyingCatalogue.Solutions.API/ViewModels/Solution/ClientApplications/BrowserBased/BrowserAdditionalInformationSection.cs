using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserAdditionalInformationSection
    {
        public BrowserAdditionalInformationSection(IClientApplication clientApplication) =>
            Answers = new BrowserAdditionalInformationSectionAnswers(clientApplication);

        public BrowserAdditionalInformationSectionAnswers Answers { get; }
    }
}
