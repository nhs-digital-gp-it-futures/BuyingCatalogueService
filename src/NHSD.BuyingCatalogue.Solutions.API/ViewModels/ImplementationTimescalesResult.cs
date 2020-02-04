using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class ImplementationTimescalesResult
    {
        [JsonProperty("description")]
        public string Description { get; }

        public ImplementationTimescalesResult(string description)
        {
            Description = description;
        }
    }
}
