using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty
{
    internal sealed class UpdateSolutionMobileThirdPartyHandler : Handler<UpdateSolutionMobileThirdPartyCommand, ISimpleResult>
    {
        public UpdateSolutionMobileThirdPartyHandler(IExecutor<UpdateSolutionMobileThirdPartyCommand> executor, IValidator<UpdateSolutionMobileThirdPartyCommand, ISimpleResult> validator) : base(executor, validator)
        {
        }
    }
}
