namespace NHSD.BuyingCatalogue.Solutions.Contracts.Epics
{
    public interface IClaimedEpic
    {
        string EpicId { get; }

        string StatusName { get; }
    }
}
