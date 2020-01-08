using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public class NativeDesktopOperatingSystemsSection
    {
        public NativeDesktopOperatingSystemsSectionAnswers Answers { get; }

        public NativeDesktopOperatingSystemsSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopOperatingSystemsSectionAnswers(clientApplication);
    }
}
