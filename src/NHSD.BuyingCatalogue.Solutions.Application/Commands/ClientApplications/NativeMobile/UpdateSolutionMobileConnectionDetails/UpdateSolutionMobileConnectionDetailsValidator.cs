using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    internal sealed class UpdateSolutionMobileConnectionDetailsValidator : IValidator<UpdateSolutionMobileConnectionDetailsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionMobileConnectionDetailsCommand command) =>
            new MaxLengthValidator().Validate(command.Data.ConnectionRequirementsDescription, 300, "connection-requirements-description").Result();
    }
}
