using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionClientApplicationTypesResult
    {
        internal UpdateSolutionClientApplicationTypesResult(RequiredResult updateSolutionClientApplicationTypesValidationResult)
        {
            Required = updateSolutionClientApplicationTypesValidationResult.Required;
        }

        public HashSet<string> Required { get; }
    }
}
