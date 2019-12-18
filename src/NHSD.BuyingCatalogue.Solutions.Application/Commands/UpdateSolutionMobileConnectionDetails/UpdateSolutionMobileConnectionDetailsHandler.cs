using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails
{
    class UpdateSolutionMobileConnectionDetailsHandler : IRequestHandler<UpdateSolutionMobileConnectionDetailsCommand, MaxLengthResult>
    {
        private readonly ClientApplicationPartialUpdater _updater;
        private readonly UpdateSolutionMobileConnectionDetailsValidator _validator;

        public UpdateSolutionMobileConnectionDetailsHandler(ClientApplicationPartialUpdater updater, UpdateSolutionMobileConnectionDetailsValidator validator)
        {
            _updater = updater;
            _validator = validator;
        }

        public async Task<MaxLengthResult> Handle(UpdateSolutionMobileConnectionDetailsCommand request, CancellationToken cancellationToken)
        {
            var result = _validator.Validate(request.Details);

            if (result.IsValid)
            {
                await _updater.UpdateAsync(request.SolutionId, application =>
                        {
                            application.MobileConnectionDetails = new MobileConnectionDetails
                            {
                                MinimumConnectionSpeed = request.Details.MinimumConnectionSpeed,
                                Description = request.Details.ConnectionRequirementsDescription,
                                ConnectionType = request.Details.ConnectionType
                            };
                        },
                    cancellationToken)
                    .ConfigureAwait(false);
            }

            return result;
        }
    }
}
