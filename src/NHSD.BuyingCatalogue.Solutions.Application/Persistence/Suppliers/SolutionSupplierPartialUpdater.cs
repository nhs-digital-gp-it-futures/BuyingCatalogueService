using System;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SolutionSupplierPartialUpdater
    {
        private readonly SupplierReader _supplierReader;
        private readonly SolutionSupplierUpdater solutionSupplierUpdater;

        public SolutionSupplierPartialUpdater(SupplierReader supplierReader, SolutionSupplierUpdater solutionSolutionSupplierUpdater)
        {
            _supplierReader = supplierReader;
            solutionSupplierUpdater = solutionSolutionSupplierUpdater;
        }

        public async Task UpdateAsync(string solutionId, Action<SolutionSupplier> updateAction,
            CancellationToken cancellationToken)
        {
            var supplier =
                await _supplierReader.BySolutionIdAsync(solutionId, cancellationToken).ConfigureAwait(false);

            updateAction(supplier);

            await solutionSupplierUpdater.UpdateBySolutionId(solutionId, supplier, cancellationToken).ConfigureAwait(false);
        }
    }
}
