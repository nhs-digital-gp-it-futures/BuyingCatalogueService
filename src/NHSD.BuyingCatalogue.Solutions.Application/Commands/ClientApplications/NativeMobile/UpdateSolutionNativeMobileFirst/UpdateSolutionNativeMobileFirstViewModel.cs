using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst
{
    public class UpdateSolutionNativeMobileFirstViewModel
    {
        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; set; }
    }
}
