using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public sealed class NativeDesktopConnectivityDetailsSection
    {
        public NativeDesktopConnectivityDetailsSectionAnswers Answers { get; }

        public NativeDesktopConnectivityDetailsSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopConnectivityDetailsSectionAnswers(clientApplication);
    }
}
