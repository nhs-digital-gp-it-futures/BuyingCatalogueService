using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Capabilities.Contracts;

namespace NHSD.BuyingCatalogue.Capabilities.API.ViewModels
{
    /// <summary>
    /// Provides the view representation for the <see cref="Capability"/> entity.
    /// </summary>
    public sealed class CapabilityViewModel
    {
        internal CapabilityViewModel(ICapability capability)
        {
            CapabilityReference = capability.CapabilityReference;
            Version = capability.Version;
            Name = capability.Name;
            IsFoundation = capability.IsFoundation;
        }

        /// <summary>
        /// Capability Reference
        /// </summary>
        [JsonProperty("reference")]
        public string CapabilityReference { get; }

        /// <summary>
        /// Version of the Capability
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A value to determine whether or not this instance is the criteria to form a foundation solution.
        /// </summary>
        public bool IsFoundation { get; }
    }
}
