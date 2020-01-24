using System;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class SupplierPartialUpdater
    {
        private readonly SupplierReader _supplierReader;
        private readonly SupplierUpdater _solutionSupplierUpdater;

        public SupplierPartialUpdater(SupplierReader supplierReader, SupplierUpdater solutionSupplierUpdater)
        {
            _supplierReader = supplierReader;
            _solutionSupplierUpdater = solutionSupplierUpdater;
        }

        public async Task UpdateAsync(string solutionId, Action<Supplier> updateAction,
            CancellationToken cancellationToken)
        {
            var supplier =
                (await _supplierReader.BySolutionIdAsync(solutionId, cancellationToken).ConfigureAwait(false));

            updateAction(supplier);

            await _solutionSupplierUpdater.Update(solutionId, supplier, cancellationToken).ConfigureAwait(false);
        }
    }
}
