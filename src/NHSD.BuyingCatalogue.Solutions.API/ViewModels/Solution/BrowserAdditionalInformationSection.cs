using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class BrowserAdditionalInformationSection
    {
        public BrowserAdditionalInformationSectionAnswers Answers { get; }

        public BrowserAdditionalInformationSection(IClientApplication clientApplication) =>
            Answers = new BrowserAdditionalInformationSectionAnswers(clientApplication);
    }
}
