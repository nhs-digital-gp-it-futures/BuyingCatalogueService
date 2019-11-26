using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionClientApplicationTypesResult
    {
        public UpdateSolutionClientApplicationTypesResult(UpdateSolutionClientApplicationTypesValidationResult updateSolutionClientApplicationTypesValidationResult)
        {
            Required = updateSolutionClientApplicationTypesValidationResult.Required;
        }

        public HashSet<string> Required { get; }
    }
}
