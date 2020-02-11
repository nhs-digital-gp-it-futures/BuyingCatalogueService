using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateEpics
{
    internal sealed class UpdateEpicsValidator : IValidator<UpdateEpicsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateEpicsCommand command)
        {
            return new RequiredResult();
        }
    }
}
