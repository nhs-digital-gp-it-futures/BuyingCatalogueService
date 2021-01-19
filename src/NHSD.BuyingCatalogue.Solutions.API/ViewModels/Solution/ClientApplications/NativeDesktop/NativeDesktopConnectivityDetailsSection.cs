using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopConnectivityDetailsSection
    {
        public NativeDesktopConnectivityDetailsSection(IClientApplication clientApplication) =>
            Answers = new NativeDesktopConnectivityDetailsSectionAnswers(clientApplication);

        public NativeDesktopConnectivityDetailsSectionAnswers Answers { get; }
    }
}
