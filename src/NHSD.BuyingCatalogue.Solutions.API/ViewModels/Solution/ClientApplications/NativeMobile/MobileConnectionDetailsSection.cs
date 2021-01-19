using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileConnectionDetailsSection
    {
        public MobileConnectionDetailsSection(IClientApplication clientApplication)
        {
            Answers = new MobileConnectionDetailsSectionAnswers(clientApplication?.MobileConnectionDetails);
        }

        public MobileConnectionDetailsSectionAnswers Answers { get; }
    }
}
