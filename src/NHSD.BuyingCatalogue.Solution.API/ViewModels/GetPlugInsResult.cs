using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetPlugInsResult
    {
        [JsonProperty("plugins-required")]
        public string PlugIns { get; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get;}

        public GetPlugInsResult(IPlugins plugins)
        {
            PlugIns = plugins?.Required.ToYesNoString();
            AdditionalInformation = plugins?.AdditionalInformation;
        }
    }
}
