using System.Collections.Generic;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;

namespace NHSD.BuyingCatalogue.Solution.API.ViewModels
{
    public class UpdateSolutionSummaryResult
    {
        public HashSet<string> Required { get; }

        public HashSet<string> MaxLength { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionSummaryResult"/> class.
        /// </summary>
        public UpdateSolutionSummaryResult(UpdateSolutionSummaryValidationResult updateSolutionSummaryValidationResult)
        {
            Required = updateSolutionSummaryValidationResult.Required;
            MaxLength = updateSolutionSummaryValidationResult.MaxLength;
        }
    }
}
