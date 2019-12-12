using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    public class UpdateSolutionBrowserMobileFirstViewModel
    {
        [JsonProperty("mobile-first-design")]
        public bool? MobileFirstDesign { get; set; }
    }
}
