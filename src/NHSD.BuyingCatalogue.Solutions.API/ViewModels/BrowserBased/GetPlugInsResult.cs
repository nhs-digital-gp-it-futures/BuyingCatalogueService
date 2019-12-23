using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

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
