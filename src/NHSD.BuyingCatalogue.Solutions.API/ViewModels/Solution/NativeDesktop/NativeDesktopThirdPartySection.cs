using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public sealed class NativeDesktopThirdPartySection
    {
        public NativeDesktopThirdPartySectionAnswers Answers { get; }

        internal NativeDesktopThirdPartySection(IClientApplication clientApplication)
        {
            Answers = new NativeDesktopThirdPartySectionAnswers(clientApplication.NativeDesktopThirdParty);
        }
    }
}
