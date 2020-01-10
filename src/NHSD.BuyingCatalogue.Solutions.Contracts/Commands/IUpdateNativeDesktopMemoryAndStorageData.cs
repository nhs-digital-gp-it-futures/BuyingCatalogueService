namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateNativeDesktopMemoryAndStorageData
    {
        string MinimumMemoryRequirement { get; }
        string StorageRequirementsDescription { get; }
        string MinimumCpu { get; }
        string RecommendedResolution { get; }

        IUpdateNativeDesktopMemoryAndStorageData Trim();
    }
}
