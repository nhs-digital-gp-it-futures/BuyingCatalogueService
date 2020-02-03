using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations
{
    internal sealed class UpdateIntegrationsValidator : IValidator<UpdateIntegrationsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateIntegrationsCommand command)
            => new MaxLengthValidator()
                .Validate(command.Url, 1000, "link")
                .Result();
    }
}
