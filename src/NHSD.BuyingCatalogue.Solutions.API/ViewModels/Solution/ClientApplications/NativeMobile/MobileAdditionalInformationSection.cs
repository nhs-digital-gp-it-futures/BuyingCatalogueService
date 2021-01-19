using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileAdditionalInformationSection
    {
        public MobileAdditionalInformationSection(IClientApplication clientApplication) =>
            Answers = new MobileAdditionalInformationSectionAnswers(clientApplication);

        public MobileAdditionalInformationSectionAnswers Answers { get; }
    }
}
