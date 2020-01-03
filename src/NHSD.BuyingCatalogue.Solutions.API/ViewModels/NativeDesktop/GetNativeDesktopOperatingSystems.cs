using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class GetNativeDesktopOperatingSystems
    {
        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; set; }
    }
}
