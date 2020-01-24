using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class GetSupplierResult
    {
        [JsonProperty("description")]
        public string SupplierDescription { get; set; }

        [JsonProperty("link")]
        public string SupplierLink { get; set; }

        public GetSupplierResult(ISupplier aboutSupplier)
        {
            SupplierDescription = aboutSupplier?.Summary;
            SupplierLink = aboutSupplier?.Url;
        }
    }
}
