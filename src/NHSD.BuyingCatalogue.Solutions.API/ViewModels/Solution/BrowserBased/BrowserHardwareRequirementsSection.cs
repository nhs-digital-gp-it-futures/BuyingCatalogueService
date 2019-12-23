using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class BrowserHardwareRequirementsSection
    {
        public BrowserHardwareRequirementsSectionAnswers Answers { get; }

        public BrowserHardwareRequirementsSection(IClientApplication clientApplication) =>
            Answers = new BrowserHardwareRequirementsSectionAnswers(clientApplication);
    }
}
