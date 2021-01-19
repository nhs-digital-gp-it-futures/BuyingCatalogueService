using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopOperatingSystemsSection
    {
        public NativeDesktopOperatingSystemsSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopOperatingSystemsSectionAnswers(clientApplication);

        public NativeDesktopOperatingSystemsSectionAnswers Answers { get; }
    }
}
