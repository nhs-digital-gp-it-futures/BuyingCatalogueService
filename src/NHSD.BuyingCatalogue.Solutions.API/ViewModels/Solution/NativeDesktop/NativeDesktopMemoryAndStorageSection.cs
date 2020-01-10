using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public sealed class NativeDesktopMemoryAndStorageSection
    {
        public NativeDesktopMemoryAndStorageSectionAnswers Answers { get; }

        internal NativeDesktopMemoryAndStorageSection(IClientApplication clientApplication)
        {
            Answers = new NativeDesktopMemoryAndStorageSectionAnswers(clientApplication.NativeDesktopMemoryAndStorage);
        }
    }
}
