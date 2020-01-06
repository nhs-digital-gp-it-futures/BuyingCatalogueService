using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    internal sealed class UpdateSolutionMobileOperatingSystemsHandler : Handler<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult>
    {
        public UpdateSolutionMobileOperatingSystemsHandler(IExecutor<UpdateSolutionMobileOperatingSystemsCommand> updateSolutionMobileOperatingSystemsyExecutor,
            IValidator<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult> updateSolutionMobileOperatingSystemsValidator) : base(updateSolutionMobileOperatingSystemsyExecutor, updateSolutionMobileOperatingSystemsValidator)
        {
        }
    }
}
