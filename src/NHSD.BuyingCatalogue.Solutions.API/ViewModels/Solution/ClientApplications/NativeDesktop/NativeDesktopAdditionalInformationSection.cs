using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopAdditionalInformationSection
    {
        public NativeDesktopAdditionalInformationSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopAdditionalInformationSectionAnswer(clientApplication);

        public NativeDesktopAdditionalInformationSectionAnswer Answers { get; }
    }
}
