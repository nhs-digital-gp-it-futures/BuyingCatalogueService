using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class GetSupplierResult
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        public GetSupplierResult(ISupplier aboutSupplier)
        {
            Description = aboutSupplier?.Description;
            Link = aboutSupplier?.Link;
        }
    }
}
