using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public class MobileOperatingSystemsSection
    {
        public MobileOperatingSystemsSectionAnswers Answers { get; }

        public MobileOperatingSystemsSection(IClientApplication clientApplication)
        {
            Answers = new MobileOperatingSystemsSectionAnswers(clientApplication);
        }
    }
}
