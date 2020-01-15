using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public class BrowserHardwareRequirementsSection
    {
        public BrowserHardwareRequirementsSectionAnswers Answers { get; }

        public BrowserHardwareRequirementsSection(IClientApplication clientApplication) =>
            Answers = new BrowserHardwareRequirementsSectionAnswers(clientApplication);
    }
}
