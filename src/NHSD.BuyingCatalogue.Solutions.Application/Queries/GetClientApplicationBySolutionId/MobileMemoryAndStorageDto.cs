using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    public sealed class MobileMemoryAndStorageDto : IMobileMemoryAndStorage
    {
        public string MinimumMemoryRequirement { get; internal set; }

        public string Description { get; internal set; }
    }
}
