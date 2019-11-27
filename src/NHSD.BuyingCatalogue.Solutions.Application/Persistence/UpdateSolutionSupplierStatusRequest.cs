using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateSolutionSupplierStatusRequest : IUpdateSolutionSupplierStatusRequest
    {
        public UpdateSolutionSupplierStatusRequest(string id, int supplierStatusId)
        {
            Id = id;
            SupplierStatusId = supplierStatusId;
        }

        public string Id { get; }

        public int SupplierStatusId { get; }
    }
}
