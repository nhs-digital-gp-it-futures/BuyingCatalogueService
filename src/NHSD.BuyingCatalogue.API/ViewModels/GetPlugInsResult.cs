using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class GetPlugInsResult
    {
        [JsonProperty("plug-ins")]
        public string PlugIns { get; set; }

        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; set; }
    }
}
