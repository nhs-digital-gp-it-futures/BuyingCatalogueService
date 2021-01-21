namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IClaimedCapabilityEpic
    {
        string EpicId { get; }

        string EpicName { get; }

        string EpicCompliancyLevel { get; }

        bool IsMet { get; }
    }
}
