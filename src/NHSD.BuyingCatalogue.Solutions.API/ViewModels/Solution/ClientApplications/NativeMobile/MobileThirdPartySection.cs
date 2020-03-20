using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public class MobileThirdPartySection
    {
        public MobileThirdSectionAnswers Answers { get; }

        public MobileThirdPartySection(IClientApplication clientApplication)
        {
            Answers = new MobileThirdSectionAnswers(clientApplication?.MobileThirdParty);
        }
    }
}
