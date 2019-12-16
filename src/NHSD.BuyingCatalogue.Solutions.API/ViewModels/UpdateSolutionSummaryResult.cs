using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionSummaryResult
    {
        public HashSet<string> Required { get; }

        public HashSet<string> MaxLength { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionSummaryResult"/> class.
        /// </summary>
        internal UpdateSolutionSummaryResult(RequiredMaxLengthResult updateSolutionSummaryValidationResult)
        {
            Required = updateSolutionSummaryValidationResult.Required;
            MaxLength = updateSolutionSummaryValidationResult.MaxLength;
        }
    }
}
