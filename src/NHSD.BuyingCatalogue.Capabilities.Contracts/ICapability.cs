namespace NHSD.BuyingCatalogue.Capabilities.Contracts
{
    public interface ICapability
    {
        /// <summary>
        /// Capability Reference
        /// </summary>
        string CapabilityReference { get; }

        /// <summary>
        /// Version of the Capability
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A value to determine whether or not this instance is the criteria to form a foundation solution.
        /// </summary>
        bool IsFoundation { get; }
    }
}
