using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics.UpdateEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics
{
    internal sealed class UpdateClaimedEpicsValidator : IValidator<UpdateClaimedEpicsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateClaimedEpicsCommand command)
        {
            return new RequiredResult();
        }
    }
}
