using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopOperatingSystemsResult
    {
        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; set; }

        public GetNativeDesktopOperatingSystemsResult(string operatingSystemsDescription)
        {
            OperatingSystemsDescription = operatingSystemsDescription;
        }
    }
}
