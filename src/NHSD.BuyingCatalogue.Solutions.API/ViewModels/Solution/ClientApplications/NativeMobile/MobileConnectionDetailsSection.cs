using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public class MobileConnectionDetailsSection
    {
        public MobileConnectionDetailsSectionAnswers Answers { get; }

        public MobileConnectionDetailsSection(IClientApplication clientApplication)
        {
            Answers = new MobileConnectionDetailsSectionAnswers(clientApplication?.MobileConnectionDetails);
        }
    }
}
