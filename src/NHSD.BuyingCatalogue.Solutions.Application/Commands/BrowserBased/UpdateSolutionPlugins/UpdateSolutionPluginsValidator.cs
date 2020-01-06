using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator : IValidator<UpdateSolutionPluginsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionPluginsCommand updateSolutionPluginsCommand)
            => new RequiredMaxLengthResult(
                new RequiredValidator().Validate(updateSolutionPluginsCommand.Data.Required, "plugins-required").Result(),
                new MaxLengthValidator().Validate(updateSolutionPluginsCommand.Data.AdditionalInformation, 500, "plugins-detail").Result()
            );
    }
}
