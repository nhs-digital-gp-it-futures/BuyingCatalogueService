using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Capabilities.Contracts;

namespace NHSD.BuyingCatalogue.Capabilities.API.ViewModels
{
    /// <summary>
    /// Provides the view representation for the <see cref="ICapability"/> entity.
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
        /// Gets the capability reference.
        /// </summary>
        [JsonProperty("reference")]
        public string CapabilityReference { get; }

        /// <summary>
        /// Gets the version of the capability.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the name of the capability.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether or not this instance is the criteria to form a foundation solution.
        /// </summary>
        public bool IsFoundation { get; }
    }
}
