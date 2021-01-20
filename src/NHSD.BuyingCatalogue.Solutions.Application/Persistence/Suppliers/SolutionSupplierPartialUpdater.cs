using System;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SolutionSupplierPartialUpdater
    {
        private readonly SupplierReader supplierReader;
        private readonly SolutionSupplierUpdater solutionSupplierUpdater;

        public SolutionSupplierPartialUpdater(SupplierReader supplierReader, SolutionSupplierUpdater solutionSupplierUpdater)
        {
            this.supplierReader = supplierReader;
            this.solutionSupplierUpdater = solutionSupplierUpdater;
        }

        public async Task UpdateAsync(
            string solutionId,
            Action<SolutionSupplier> updateAction,
            CancellationToken cancellationToken)
        {
            var supplier = await supplierReader.BySolutionIdAsync(solutionId, cancellationToken);

            updateAction(supplier);

            await solutionSupplierUpdater.UpdateBySolutionId(solutionId, supplier, cancellationToken);
        }
    }
}
