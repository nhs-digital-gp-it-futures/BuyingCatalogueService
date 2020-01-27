using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers
{
    internal sealed class UpdateSupplierExecutor : IExecutor<UpdateSupplierCommand>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SupplierPartialUpdater _updater;

        public UpdateSupplierExecutor(SolutionVerifier verifier, SupplierPartialUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {

            await _verifier.ThrowWhenMissing(request.SolutionId, cancellationToken).ConfigureAwait(false);

            await _updater.UpdateAsync(request.SolutionId, supplier =>
            {
                supplier.Description = request.Data.Description;
                supplier.Link = request.Data.Link;
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
