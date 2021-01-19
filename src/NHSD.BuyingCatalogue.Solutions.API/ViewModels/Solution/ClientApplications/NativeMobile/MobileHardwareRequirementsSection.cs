using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileHardwareRequirementsSection
    {
        public MobileHardwareRequirementsSection(IClientApplication clientApplication) =>
            Answers = new MobileHardwareRequirementsSectionAnswers(clientApplication);

        public MobileHardwareRequirementsSectionAnswers Answers { get; }
    }
}
