using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class AboutSupplierSectionAnswers
    {
        [JsonProperty("description")]
        public string Description { get; }

        [JsonProperty("link")]
        public string Link { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Description) ||
                               !string.IsNullOrWhiteSpace(Link);

        public AboutSupplierSectionAnswers(ISolutionSupplier solutionSupplier)
        {
            Description = solutionSupplier?.Summary;
            Link = solutionSupplier?.Url;
        }
    }
}
