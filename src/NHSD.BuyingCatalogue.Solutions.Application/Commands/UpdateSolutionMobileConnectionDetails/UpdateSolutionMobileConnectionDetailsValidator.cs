using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails
{
    internal sealed class UpdateSolutionMobileConnectionDetailsValidator : IValidator<UpdateSolutionMobileConnectionDetailsCommand, MaxLengthResult>
    {
        public MaxLengthResult Validate(UpdateSolutionMobileConnectionDetailsCommand updateSolutionMobileConnectionDetailsCommand) =>
            new MaxLengthValidator().Validate(updateSolutionMobileConnectionDetailsCommand.Details.ConnectionRequirementsDescription, 300, "connection-requirements-description").Result();
    }
}
