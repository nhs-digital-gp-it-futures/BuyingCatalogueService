using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserHardwareRequirementsSection
    {
        public BrowserHardwareRequirementsSection(IClientApplication clientApplication) =>
            Answers = new BrowserHardwareRequirementsSectionAnswers(clientApplication);

        public BrowserHardwareRequirementsSectionAnswers Answers { get; }
    }
}
