using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities
{
    internal sealed class UpdateCapabilitiesValidator : IValidator<UpdateCapabilitiesCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateCapabilitiesCommand command)
        {
            return new RequiredResult();
        }
    }
}
