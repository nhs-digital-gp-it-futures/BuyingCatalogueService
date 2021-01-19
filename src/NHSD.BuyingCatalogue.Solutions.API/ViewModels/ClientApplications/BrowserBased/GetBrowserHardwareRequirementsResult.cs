using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class GetBrowserHardwareRequirementsResult
    {
        public GetBrowserHardwareRequirementsResult(string requirement)
        {
            HardwareRequirements = requirement;
        }

        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; }
    }
}
