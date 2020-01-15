using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopHardwareRequirementsSection
    {
        public NativeDesktopHardwareRequirementsSectionAnswers Answers { get; }

        public NativeDesktopHardwareRequirementsSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopHardwareRequirementsSectionAnswers(clientApplication);
    }
}
