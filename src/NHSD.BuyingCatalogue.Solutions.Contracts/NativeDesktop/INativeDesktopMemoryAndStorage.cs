namespace NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop
{
    public interface INativeDesktopMemoryAndStorage
    {
        string MinimumMemoryRequirement { get; }

        string StorageRequirementsDescription { get; }

        string MinimumCpu { get; }

        string RecommendedResolution { get; }
    }
}
