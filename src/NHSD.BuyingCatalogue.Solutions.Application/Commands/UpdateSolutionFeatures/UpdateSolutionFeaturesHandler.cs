using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures
{
    internal sealed class UpdateSolutionFeaturesHandler : Handler<UpdateSolutionFeaturesCommand, MaxLengthResult>
    {
        public UpdateSolutionFeaturesHandler(IExecutor<UpdateSolutionFeaturesCommand> updateSolutionFeaturesExecutor,
            IValidator<UpdateSolutionFeaturesCommand, MaxLengthResult> updateSolutionFeaturesValidator) : base(updateSolutionFeaturesExecutor, updateSolutionFeaturesValidator)
        {
        }
    }
}
