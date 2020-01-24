using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SupplierUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISupplierRepository _supplierRepository;

        public SupplierUpdater(ISupplierRepository supplierRepository)
            => _supplierRepository = supplierRepository;

        public async Task Update(string solutionId, Domain.Suppliers.Supplier supplier, CancellationToken cancellationToken) =>
            await _supplierRepository
                .UpdateSupplierAsync(new UpdateSupplierRequest(solutionId, supplier.Description, supplier.Link),
                    cancellationToken).ConfigureAwait(false);
    }
}
