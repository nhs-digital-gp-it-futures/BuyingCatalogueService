using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SolutionSupplierUpdater
    {
        private readonly ISupplierRepository _supplierRepository;

        public SolutionSupplierUpdater(ISupplierRepository supplierRepository)
            => _supplierRepository = supplierRepository;

        public async Task UpdateBySolutionId(string solutionId, SolutionSupplier solutionSupplier, CancellationToken cancellationToken) =>
            await _supplierRepository
                .UpdateSupplierAsync(new UpdateSupplierRequest(solutionId, solutionSupplier.Summary, solutionSupplier.Url),
                    cancellationToken).ConfigureAwait(false);
    }
}
