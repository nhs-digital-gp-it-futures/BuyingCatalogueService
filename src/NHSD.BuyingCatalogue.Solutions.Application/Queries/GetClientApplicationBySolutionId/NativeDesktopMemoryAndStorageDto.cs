using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    public sealed class NativeDesktopMemoryAndStorageDto : INativeDesktopMemoryAndStorage
    {
        public string MinimumMemoryRequirement { get; internal set; }

        public string StorageRequirementsDescription { get; internal set; }

        public string MinimumCpu { get; internal set; }

        public string RecommendedResolution { get; internal set; }
    }
}
