using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    public class ClaimedCapabilityEpicDto : IClaimedCapabilityEpic
    {
        public string EpicId { get; set; }
        public string EpicName { get; set; }
        public string EpicCompliancyLevel { get; set; }
        public bool IsMet { get; set; }
    }
}
