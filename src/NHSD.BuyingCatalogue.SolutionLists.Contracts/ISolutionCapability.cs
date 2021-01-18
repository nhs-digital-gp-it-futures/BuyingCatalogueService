namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionCapability
    {
        /// <summary>
        /// Gets the ID of the capability.
        /// </summary>
        string CapabilityReference { get; }

        /// <summary>
        /// Gets the name of the capability.
        /// </summary>
        string Name { get; }
    }
}
