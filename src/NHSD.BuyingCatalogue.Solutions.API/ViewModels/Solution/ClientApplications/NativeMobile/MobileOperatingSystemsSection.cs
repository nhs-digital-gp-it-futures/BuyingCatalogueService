using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileOperatingSystemsSection
    {
        public MobileOperatingSystemsSection(IClientApplication clientApplication)
        {
            Answers = new MobileOperatingSystemsSectionAnswers(clientApplication);
        }

        public MobileOperatingSystemsSectionAnswers Answers { get; }
    }
}
