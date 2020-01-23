using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SupplierReader
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierReader(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Supplier> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            return new Supplier(await _supplierRepository.GetSupplierBySolutionIdAsync(id, cancellationToken)
                .ConfigureAwait(false));
        }
    }
}
