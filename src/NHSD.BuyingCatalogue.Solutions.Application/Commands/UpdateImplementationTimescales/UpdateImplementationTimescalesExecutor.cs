using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales
{
    internal sealed class UpdateImplementationTimescalesExecutor : IExecutor<UpdateImplementationTimescalesCommand>
    {
        private readonly SolutionVerifier verifier;
        private readonly SolutionImplementationTimescalesUpdater updater;

        public UpdateImplementationTimescalesExecutor(SolutionVerifier verifier, SolutionImplementationTimescalesUpdater updater)
        {
            this.verifier = verifier;
            this.updater = updater;
        }

        public async Task UpdateAsync(UpdateImplementationTimescalesCommand request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken);
            await updater.Update(request.SolutionId, request.Description, cancellationToken);
        }
    }
}
