using System.Collections.Generic;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes;

namespace NHSD.BuyingCatalogue.API.ViewModels
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
