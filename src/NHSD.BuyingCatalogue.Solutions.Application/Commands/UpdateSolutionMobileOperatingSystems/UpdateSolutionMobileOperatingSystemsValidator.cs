using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems
{
    internal class UpdateSolutionMobileOperatingSystemsValidator
    {
        public RequiredMaxLengthResult Validation(UpdateSolutionMobileOperatingSystemsViewModel viewModel) => new RequiredMaxLengthResult(
        new RequiredValidator().Validate(viewModel.OperatingSystems, "operating-systems").Result(),
        new MaxLengthValidator().Validate(viewModel.OperatingSystemsDescription, 1000, "operating-systems-description").Result()
            );
    }
}
