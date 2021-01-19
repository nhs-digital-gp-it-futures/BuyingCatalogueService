using Newtonsoft.Json;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class SolutionCapabilityResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionCapabilityResult"/> class.
        /// </summary>
        /// <param name="capability">The solution capability.</param>
        public SolutionCapabilityResult(ISolutionCapability capability)
        {
            CapabilityReference = capability?.CapabilityReference;
            Name = capability?.Name;
        }

        [JsonProperty("reference")]
        public string CapabilityReference { get; }

        public string Name { get; }
    }
}
