using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopMemoryAndStorageSection
    {
        internal NativeDesktopMemoryAndStorageSection(IClientApplication clientApplication)
        {
            Answers = new NativeDesktopMemoryAndStorageSectionAnswers(clientApplication.NativeDesktopMemoryAndStorage);
        }

        public NativeDesktopMemoryAndStorageSectionAnswers Answers { get; }
    }
}
