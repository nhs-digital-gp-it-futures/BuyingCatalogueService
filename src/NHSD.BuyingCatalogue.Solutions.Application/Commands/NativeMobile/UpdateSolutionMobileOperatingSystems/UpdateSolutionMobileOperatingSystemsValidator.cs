using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    internal class UpdateSolutionMobileOperatingSystemsValidator : IValidator<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionMobileOperatingSystemsCommand command) =>
            new RequiredMaxLengthResult(
                new RequiredValidator().Validate(command.Data.OperatingSystems, "operating-systems").Result(),
                new MaxLengthValidator().Validate(command.Data.OperatingSystemsDescription, 1000, "operating-systems-description").Result());
    }
}
