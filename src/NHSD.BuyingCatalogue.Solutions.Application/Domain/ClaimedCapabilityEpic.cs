using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class ClaimedCapabilityEpic
    {
        public string EpicId { get; }
        public string EpicName { get; }
        public string EpicCompliancyLevel { get; }
        public bool IsMet { get; }

        public ClaimedCapabilityEpic(ISolutionEpicListResult epic)
        {
            EpicId = epic.EpicId;
            EpicName = epic.EpicName;
            EpicCompliancyLevel = epic.EpicCompliancyLevel;
            IsMet = epic.IsMet;
        }
    }
}
