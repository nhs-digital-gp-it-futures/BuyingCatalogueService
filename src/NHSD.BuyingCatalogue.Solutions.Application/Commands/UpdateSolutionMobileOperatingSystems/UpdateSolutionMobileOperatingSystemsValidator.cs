using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems
{
    internal class UpdateSolutionMobileOperatingSystemsValidator : IValidator<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionMobileOperatingSystemsCommand command) =>
            new RequiredMaxLengthResult(
                new RequiredValidator().Validate(command.ViewModel.OperatingSystems, "operating-systems").Result(),
                new MaxLengthValidator().Validate(command.ViewModel.OperatingSystemsDescription, 1000, "operating-systems-description").Result());
    }
}
