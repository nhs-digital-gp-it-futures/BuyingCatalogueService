using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator : IValidator<UpdateSolutionPluginsCommand, RequiredMaxLengthResult>
    {
        public RequiredMaxLengthResult Validate(UpdateSolutionPluginsCommand updateSolutionPluginsCommand)
            => new RequiredMaxLengthResult(
                new RequiredValidator().Validate(updateSolutionPluginsCommand.UpdateSolutionPluginsViewModel.Required, "plugins-required").Result(),
                new MaxLengthValidator().Validate(updateSolutionPluginsCommand.UpdateSolutionPluginsViewModel.AdditionalInformation, 500, "plugins-detail").Result()
            );
    }
}
