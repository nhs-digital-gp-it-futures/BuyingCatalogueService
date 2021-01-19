using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileThirdPartySection
    {
        public MobileThirdPartySection(IClientApplication clientApplication)
        {
            Answers = new MobileThirdSectionAnswers(clientApplication?.MobileThirdParty);
        }

        public MobileThirdSectionAnswers Answers { get; }
    }
}
