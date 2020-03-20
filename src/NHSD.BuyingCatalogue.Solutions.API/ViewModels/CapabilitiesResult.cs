using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class CapabilitiesResult
    {
        [JsonProperty("solution-id")]
        public string SolutionId { get; set; }
    }
}
