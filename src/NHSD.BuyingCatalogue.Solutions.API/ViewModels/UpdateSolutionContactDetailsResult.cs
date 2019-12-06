using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionContactDetailsResult
    {
        internal UpdateSolutionContactDetailsResult(UpdateSolutionContactDetailsValidationResult updateSolutionFeaturesValidationResult)
        {
            MaxLength = updateSolutionFeaturesValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }
    }
}
