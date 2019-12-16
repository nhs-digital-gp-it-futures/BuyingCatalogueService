using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionBrowserAdditionalInformationResult
    {
        internal UpdateSolutionBrowserAdditionalInformationResult(MaxLengthResult updateSolutionBrowserAdditionalInformationValidationResult)
        {
            MaxLength = updateSolutionBrowserAdditionalInformationValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }
    }
}
