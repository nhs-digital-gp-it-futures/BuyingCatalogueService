using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class GetNativeDesktopOperatingSystemsResult
    {
        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; set; }
    }
}
