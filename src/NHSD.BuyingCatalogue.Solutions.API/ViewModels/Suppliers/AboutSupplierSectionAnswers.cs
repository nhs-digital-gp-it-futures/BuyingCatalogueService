using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class AboutSupplierSectionAnswers
    {
        [JsonProperty("description")]
        public string SupplierSummary { get; }

        [JsonProperty("link")]
        public string SupplierUrl { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(SupplierSummary) ||
                               !string.IsNullOrWhiteSpace(SupplierUrl);

        public AboutSupplierSectionAnswers(ISupplier supplier)
        {
            SupplierSummary = supplier?.Description;
            SupplierUrl = supplier?.Link;
        }

    }
}
