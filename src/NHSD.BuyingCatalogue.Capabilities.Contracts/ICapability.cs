namespace NHSD.BuyingCatalogue.Capabilities.Contracts
{
    public interface ICapability
    {
        /// <summary>
        /// Gets the capability reference.
        /// </summary>
        string CapabilityReference { get; }

        /// <summary>
        /// Gets the version of the capability.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the name of the capability.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether or not this instance is the criteria to form a foundation solution.
        /// </summary>
        bool IsFoundation { get; }
    }
}
