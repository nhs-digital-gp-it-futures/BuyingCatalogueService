using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    internal sealed class UpdateSolutionMobileOperatingSystemsHandler : Handler<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult>
    {
        public UpdateSolutionMobileOperatingSystemsHandler(
            IExecutor<UpdateSolutionMobileOperatingSystemsCommand> updateSolutionMobileOperatingSystemsExecutor,
            IValidator<UpdateSolutionMobileOperatingSystemsCommand, ISimpleResult> updateSolutionMobileOperatingSystemsValidator)
            : base(updateSolutionMobileOperatingSystemsExecutor, updateSolutionMobileOperatingSystemsValidator)
        {
        }
    }
}
