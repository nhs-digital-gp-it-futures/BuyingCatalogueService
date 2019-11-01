using System.Collections.Generic;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class UpdateSolutionSummaryResult
    {
        public UpdateSolutionSummaryResult(UpdateSolutionSummaryValidationResult updateSolutionSummaryValidationResult)
        {
            Required = updateSolutionSummaryValidationResult.Required;
            MaxLength = updateSolutionSummaryValidationResult.MaxLength;
        }

        public HashSet<string> Required { get; }

        public HashSet<string> MaxLength { get; }
    }
}
