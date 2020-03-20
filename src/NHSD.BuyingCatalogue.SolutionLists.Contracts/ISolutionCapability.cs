namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionCapability
    {
        /// <summary>
        /// Identifier of the capability.
        /// </summary>
        string CapabilityReference { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        string Name { get; }
    }
}
