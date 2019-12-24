using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased
{
    public sealed class GetBrowserHardwareRequirementsResult
    {
        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; }

        public GetBrowserHardwareRequirementsResult(string requirement)
        {
            HardwareRequirements = requirement;
        }
    }
}
