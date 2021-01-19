using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class NativeMobileFirstSection
    {
        public NativeMobileFirstSection(IClientApplication clientApplication) =>
            Answers = new NativeMobileFirstSectionAnswers(clientApplication);

        public NativeMobileFirstSectionAnswers Answers { get; }
    }
}
