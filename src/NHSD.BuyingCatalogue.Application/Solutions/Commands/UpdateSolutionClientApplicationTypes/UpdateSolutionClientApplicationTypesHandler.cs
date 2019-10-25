using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    internal sealed class UpdateSolutionClientApplicationTypesHandler : IRequestHandler<UpdateSolutionClientApplicationTypesCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private static readonly HashSet<string> _acceptedClientApplicationTypes = new HashSet<string> { "browser-based", "native-mobile", "native-desktop" };

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionClientApplicationTypesHandler"/> class.
        /// </summary>
        public UpdateSolutionClientApplicationTypesHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<Unit> Handle(UpdateSolutionClientApplicationTypesCommand request, CancellationToken cancellationToken)
        {
            var filteredClientApplicationTypes = request
                .UpdateSolutionClientApplicationTypesViewModel.ClientApplicationTypes
                .Where(s => _acceptedClientApplicationTypes.Contains(s));

            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId,
                clientApplication => clientApplication.ClientApplicationTypes = new HashSet<string>(filteredClientApplicationTypes),
                cancellationToken);

            return Unit.Value;
        }
    }
}
