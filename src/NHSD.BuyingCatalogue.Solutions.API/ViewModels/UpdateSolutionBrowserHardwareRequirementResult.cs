using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionBrowserHardwareRequirementResult
    {
        internal UpdateSolutionBrowserHardwareRequirementResult(MaxLengthResult updateSolutionHardwareRequirementsValidationResult)
        {
            MaxLength = updateSolutionHardwareRequirementsValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }

    }
}
