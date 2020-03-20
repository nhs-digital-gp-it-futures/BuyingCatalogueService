using Newtonsoft.Json;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class SolutionCapabilityResult
    {
        [JsonProperty("reference")]
        public string CapabilityReference { get; }

        public string Name { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionCapabilityResult"/> class.
        /// </summary>
        public SolutionCapabilityResult(ISolutionCapability capability)
        {
            CapabilityReference = capability?.CapabilityReference;
            Name = capability?.Name;
        }
    }
}
