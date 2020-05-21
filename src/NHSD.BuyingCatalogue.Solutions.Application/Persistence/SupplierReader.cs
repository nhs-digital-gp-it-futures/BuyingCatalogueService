using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<SupplierName>> ByNameAsync(string name, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepository.GetSuppliersByName(name, cancellationToken);

            return suppliers.Select(s => new SupplierName(s));
        }

        public async Task<Supplier> BySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
        {
            return new Supplier(await _supplierRepository.GetSupplierBySolutionIdAsync(solutionId, cancellationToken)
                .ConfigureAwait(false));
        }
    }
}
