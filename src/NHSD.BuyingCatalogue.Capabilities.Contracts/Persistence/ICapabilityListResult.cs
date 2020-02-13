namespace NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence
{
    public interface ICapabilityListResult
    {
        string CapabilityReference { get; }

        string Version { get; }

        string Name { get; }

        bool IsFoundation { get; }
    }
}
