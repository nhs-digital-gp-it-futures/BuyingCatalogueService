using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.BrowserBased
{
    public class BrowserAdditionalInformationSection
    {
        public BrowserAdditionalInformationSectionAnswers Answers { get; }

        public BrowserAdditionalInformationSection(IClientApplication clientApplication) =>
            Answers = new BrowserAdditionalInformationSectionAnswers(clientApplication);
    }
}
