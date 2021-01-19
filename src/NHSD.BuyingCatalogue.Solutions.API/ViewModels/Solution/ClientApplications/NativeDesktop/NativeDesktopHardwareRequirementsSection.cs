using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopHardwareRequirementsSection
    {
        public NativeDesktopHardwareRequirementsSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopHardwareRequirementsSectionAnswers(clientApplication);

        public NativeDesktopHardwareRequirementsSectionAnswers Answers { get; }
    }
}
