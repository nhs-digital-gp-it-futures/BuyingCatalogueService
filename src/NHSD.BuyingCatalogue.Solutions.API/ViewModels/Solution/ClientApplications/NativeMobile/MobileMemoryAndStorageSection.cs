using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileMemoryAndStorageSection
    {
        public MobileMemoryAndStorageSectionAnswers Answers { get; }

        public MobileMemoryAndStorageSection(IClientApplication clientApplication) =>
            Answers = new MobileMemoryAndStorageSectionAnswers(clientApplication);
    }
}
