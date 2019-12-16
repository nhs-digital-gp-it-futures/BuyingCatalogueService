using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionFeaturesResult
    {
        internal UpdateSolutionFeaturesResult(MaxLengthResult updateSolutionFeaturesValidationResult)
        {
            MaxLength = updateSolutionFeaturesValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }
    }
}
