using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesHandler : IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private readonly UpdateSolutionClientApplicationTypesValidator _updateSolutionClientApplicationTypesValidator;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionClientApplicationTypesHandler"/> class.
        /// </summary>
        public UpdateSolutionClientApplicationTypesHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater,
            UpdateSolutionClientApplicationTypesValidator updateSolutionClientApplicationTypesValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionClientApplicationTypesValidator = updateSolutionClientApplicationTypesValidator;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<UpdateSolutionClientApplicationTypesValidationResult> Handle(UpdateSolutionClientApplicationTypesCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionClientApplicationTypesValidator.Validate(request.UpdateSolutionClientApplicationTypesViewModel);
            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId,
                    clientApplication => clientApplication.ClientApplicationTypes = new HashSet<string>(request.UpdateSolutionClientApplicationTypesViewModel.FilteredClientApplicationTypes),
                    cancellationToken);
            }

            return validationResult;
        }
    }
}
