using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers
{
    internal sealed class UpdateSolutionSupplierExecutor : IExecutor<UpdateSolutionSupplierCommand>
    {
        private readonly SolutionVerifier verifier;
        private readonly SolutionSupplierPartialUpdater updater;

        public UpdateSolutionSupplierExecutor(SolutionVerifier verifier, SolutionSupplierPartialUpdater updater)
        {
            this.verifier = verifier;
            this.updater = updater;
        }

        public async Task UpdateAsync(UpdateSolutionSupplierCommand request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken);

            await updater.UpdateAsync(
                request.SolutionId,
                supplier =>
                {
                    supplier.Summary = request.Data.Description;
                    supplier.Url = request.Data.Link;
                },
                cancellationToken);
        }
    }
}
