using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator
    {
        public RequiredMaxLengthResult Validation(UpdateSolutionPluginsViewModel updateSolutionPluginsViewModel)
            => new RequiredMaxLengthResult(
                new RequiredValidator().Validate(updateSolutionPluginsViewModel.Required, "plugins-required").Result(),
                new MaxLengthValidator().Validate(updateSolutionPluginsViewModel.AdditionalInformation, 500, "plugins-detail").Result()
            );
    }
}
