using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopOperatingSystemsResult
    {
        public GetNativeDesktopOperatingSystemsResult(string operatingSystemsDescription)
        {
            OperatingSystemsDescription = operatingSystemsDescription;
        }

        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; }
    }
}
