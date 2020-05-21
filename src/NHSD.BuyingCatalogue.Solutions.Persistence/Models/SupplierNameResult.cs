using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SupplierNameResult : ISupplierNameResult
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
