using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeMobile
{
    public class MobileAdditionalInformationSection
    {
        public MobileAdditionalInformationSectionAnswers Answers { get; }

        public MobileAdditionalInformationSection(IClientApplication clientApplication) =>
            Answers = new MobileAdditionalInformationSectionAnswers(clientApplication);
    }
}
