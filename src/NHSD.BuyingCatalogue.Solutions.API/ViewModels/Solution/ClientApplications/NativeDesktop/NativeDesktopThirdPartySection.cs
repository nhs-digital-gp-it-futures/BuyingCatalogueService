using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopThirdPartySection
    {
        internal NativeDesktopThirdPartySection(IClientApplication clientApplication)
        {
            Answers = new NativeDesktopThirdPartySectionAnswers(clientApplication.NativeDesktopThirdParty);
        }

        public NativeDesktopThirdPartySectionAnswers Answers { get; }
    }
}
