using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionFeaturesResult
    {
        public UpdateSolutionFeaturesResult(UpdateSolutionFeaturesValidationResult updateSolutionFeaturesValidationResult)
        {
            MaxLength = updateSolutionFeaturesValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }
    }
}