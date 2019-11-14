using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class GetPlugInsResult
    {
        [JsonProperty("plugins-required")]
        public string PlugIns { get; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get;}

        public GetPlugInsResult(IPlugins plugins)
        {
            bool? plug_ins = plugins?.Required;

            PlugIns = plug_ins.HasValue
                ? plug_ins.Value ? "yes" : "no"
                : null;
            AdditionalInformation = plugins?.AdditionalInformation;
        }
    }
}
