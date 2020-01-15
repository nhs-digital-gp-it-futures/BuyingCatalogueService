using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public class NativeDesktopAdditionalInformationSection
    {
        public NativeDesktopAdditionalInformationSectionAnswer Answers { get; }

        public NativeDesktopAdditionalInformationSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopAdditionalInformationSectionAnswer(clientApplication);
    }
}
