using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateImplementationTimescalesViewModel
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
