using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    internal sealed class UpdateSolutionMobileConnectionDetailsValidator : IValidator<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionMobileConnectionDetailsCommand updateSolutionMobileConnectionDetailsCommand) =>
            new MaxLengthValidator().Validate(updateSolutionMobileConnectionDetailsCommand.Details.ConnectionRequirementsDescription, 300, "connection-requirements-description").Result();
    }
}
