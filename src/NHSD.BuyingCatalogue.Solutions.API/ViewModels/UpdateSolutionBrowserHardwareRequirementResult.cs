using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionBrowserHardwareRequirementResult
    {
        internal UpdateSolutionBrowserHardwareRequirementResult(UpdateSolutionBrowserHardwareRequirementsValidationResult updateSolutionHardwareRequirementsValidationResult)
        {
            MaxLength = updateSolutionHardwareRequirementsValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }

    }
}
