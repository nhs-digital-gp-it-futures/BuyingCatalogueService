using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class UpdateSolutionSupplierStatusRequest : IUpdateSolutionSupplierStatusRequest
    {
        public string Id { get; set; }

        public int SupplierStatusId { get; set; }
    }
}
