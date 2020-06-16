namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    public interface ITimeUnit
    {
        int TimeUnitId { get; }

        string Name { get; }

        string Description { get; }
    }
}
