using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class ClaimedCapabilityEpic
    {
        public ClaimedCapabilityEpic(ISolutionEpicListResult epic)
        {
            EpicId = epic.EpicId;
            EpicName = epic.EpicName;
            EpicCompliancyLevel = epic.EpicCompliancyLevel;
            IsMet = epic.IsMet;
        }

        public string EpicId { get; }

        public string EpicName { get; }

        public string EpicCompliancyLevel { get; }

        public bool IsMet { get; }
    }
}
