using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales
{
    internal sealed class UpdateImplementationTimescalesExecutor : IExecutor<UpdateImplementationTimescalesCommand>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SolutionImplementationTimescalesUpdater _updater;

        public UpdateImplementationTimescalesExecutor(SolutionVerifier verifier, SolutionImplementationTimescalesUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateImplementationTimescalesCommand request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);
            await _updater.Update(request.SolutionId, request.Description, cancellationToken).ConfigureAwait(false);
        }
    }
}
