using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems
{
    internal sealed class UpdateSolutionMobileOperatingSystemsHandler : Handler<UpdateSolutionMobileOperatingSystemsCommand, RequiredMaxLengthResult>
    {
        public UpdateSolutionMobileOperatingSystemsHandler(IExecutor<UpdateSolutionMobileOperatingSystemsCommand> updateSolutionMobileOperatingSystemsyExecutor,
            IValidator<UpdateSolutionMobileOperatingSystemsCommand, RequiredMaxLengthResult> updateSolutionMobileOperatingSystemsValidator) : base(updateSolutionMobileOperatingSystemsyExecutor, updateSolutionMobileOperatingSystemsValidator)
        {
        }
    }
}
