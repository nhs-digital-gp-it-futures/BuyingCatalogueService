namespace NHSD.BuyingCatalogue.Solutions.Contracts.Pricing
{
    public interface IPricingUnit
    {
        string Name { get; }

        public string TierName { get; set; }

        public string Description { get; set; }
    }
}
