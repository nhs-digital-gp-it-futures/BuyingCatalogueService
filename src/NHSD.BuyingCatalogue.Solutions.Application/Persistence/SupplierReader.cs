using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SupplierReader
    {
        private readonly ISupplierRepository supplierRepository;

        public SupplierReader(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task<Supplier> ByIdAsync(string id, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetSupplierById(id, cancellationToken);

            return supplier is null ? null : new Supplier(supplier);
        }

        public async Task<IEnumerable<Supplier>> ByNameAsync(
            string name,
            PublishedStatus? solutionPublicationStatus,
            CatalogueItemType? catalogueItemType,
            CancellationToken cancellationToken)
        {
            var suppliers = await supplierRepository.GetSuppliersByNameAsync(
                name,
                solutionPublicationStatus,
                catalogueItemType,
                cancellationToken);

            return suppliers.Select(s => new Supplier(s));
        }

        public async Task<SolutionSupplier> BySolutionIdAsync(string solutionId, CancellationToken cancellationToken)
        {
            return new(await supplierRepository.GetSupplierBySolutionIdAsync(solutionId, cancellationToken));
        }
    }
}
