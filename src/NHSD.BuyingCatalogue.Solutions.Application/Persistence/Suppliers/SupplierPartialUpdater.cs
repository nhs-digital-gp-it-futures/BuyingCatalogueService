using System;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SupplierPartialUpdater
    {
        private readonly SupplierReader _supplierReader;
        private readonly SupplierUpdater _supplierUpdater;

        public SupplierPartialUpdater(SupplierReader supplierReader, SupplierUpdater solutionSupplierUpdater)
        {
            _supplierReader = supplierReader;
            _supplierUpdater = solutionSupplierUpdater;
        }

        public async Task UpdateAsync(string solutionId, Action<Supplier> updateAction,
            CancellationToken cancellationToken)
        {
            var supplier =
                (await _supplierReader.BySolutionIdAsync(solutionId, cancellationToken).ConfigureAwait(false));

            updateAction(supplier);

            await _supplierUpdater.UpdateBySolutionId(solutionId, supplier, cancellationToken).ConfigureAwait(false);
        }
    }
}
