using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class ImplementationTimescalesResult
    {
        public ImplementationTimescalesResult(string description)
        {
            Description = description;
        }

        [JsonProperty("description")]
        public string Description { get; }
    }
}
