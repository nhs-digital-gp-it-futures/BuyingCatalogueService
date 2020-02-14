using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SolutionEpicListResult : ISolutionEpicListResult
    {
        public string EpicId { get; set; }
        public Guid CapabilityId { get; set; }
        public string EpicName { get; set; }
        public string EpicCompliancyLevel { get; set; }
        public bool IsMet { get; set; }
    }
}
