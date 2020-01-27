using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SupplierUpdater
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierUpdater(ISupplierRepository supplierRepository)
            => _supplierRepository = supplierRepository;

        public async Task UpdateBySolutionId(string solutionId, Domain.Suppliers.Supplier supplier, CancellationToken cancellationToken) =>
            await _supplierRepository
                .UpdateSupplierAsync(new UpdateSupplierRequest(solutionId, supplier.Summary, supplier.Url),
                    cancellationToken).ConfigureAwait(false);
    }
}
