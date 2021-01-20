using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsValidator : IValidator<UpdateSolutionPluginsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionPluginsCommand command) => new RequiredMaxLengthResult(
            new RequiredValidator().Validate(command.Data.Required, "plugins-required").Result(),
            new MaxLengthValidator().Validate(command.Data.AdditionalInformation, 500, "plugins-detail").Result());
    }
}
