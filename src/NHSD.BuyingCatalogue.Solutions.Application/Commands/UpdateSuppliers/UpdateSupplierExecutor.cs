using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers
{
    internal sealed class UpdateSupplierExecutor : IExecutor<UpdateSolutionSupplierCommand>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SupplierPartialUpdater _updater;

        public UpdateSupplierExecutor(SolutionVerifier verifier, SupplierPartialUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateSolutionSupplierCommand request, CancellationToken cancellationToken)
        {

            await _verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);

            await _updater.UpdateAsync(request.SolutionId, supplier =>
            {
                supplier.Summary = request.Data.Description;
                supplier.Url = request.Data.Link;
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
