namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    public interface ITieredPrice
    {
        int BandStart { get; }
        int? BandEnd { get; }
        decimal Price { get; }
    }
}
