using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class AboutSupplierSectionAnswers
    {
        [JsonProperty("description")]
        public string SupplierDescription { get; }

        [JsonProperty("link")]
        public string SupplierLink { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(SupplierDescription) ||
                               !string.IsNullOrWhiteSpace(SupplierLink);

        public AboutSupplierSectionAnswers(ISupplier supplier)
        {
            SupplierDescription = supplier?.Summary;
            SupplierLink = supplier?.Url;
        }
    }
}
