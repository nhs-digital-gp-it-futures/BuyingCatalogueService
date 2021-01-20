using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SolutionSupplierUpdater
    {
        private readonly ISupplierRepository supplierRepository;

        public SolutionSupplierUpdater(ISupplierRepository supplierRepository) => this.supplierRepository = supplierRepository;

        public async Task UpdateBySolutionId(string solutionId, SolutionSupplier solutionSupplier, CancellationToken cancellationToken) =>
            await supplierRepository.UpdateSupplierAsync(
                new UpdateSupplierRequest(solutionId, solutionSupplier.Summary, solutionSupplier.Url),
                cancellationToken);
    }
}
