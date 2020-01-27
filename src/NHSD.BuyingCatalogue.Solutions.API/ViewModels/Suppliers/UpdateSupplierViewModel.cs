using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class UpdateSupplierViewModel : IUpdateSupplierData
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
