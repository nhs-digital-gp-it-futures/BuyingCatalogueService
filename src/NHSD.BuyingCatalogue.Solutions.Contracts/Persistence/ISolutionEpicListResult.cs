using System;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionEpicListResult
    {
        public string EpicId { get; }
        public Guid CapabilityId { get; set; }
        public string EpicName { get; }
        public string EpicCompliancyLevel { get; }
        public bool IsMet { get; }
    }
}
