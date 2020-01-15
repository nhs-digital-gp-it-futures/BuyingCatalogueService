using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public class MobileHardwareRequirementsSection
    {
        public MobileHardwareRequirementsSectionAnswers Answers { get; }

        public MobileHardwareRequirementsSection(IClientApplication clientApplication) =>
            Answers = new MobileHardwareRequirementsSectionAnswers(clientApplication);
    }
}
