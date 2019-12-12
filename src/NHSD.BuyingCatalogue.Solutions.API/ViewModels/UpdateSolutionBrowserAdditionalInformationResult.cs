using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionBrowserAdditionalInformationResult
    {
        internal UpdateSolutionBrowserAdditionalInformationResult(UpdateSolutionBrowserAdditionalInformationValidationResult updateSolutionBrowserAdditionalInformationValidationResult)
        {
            MaxLength = updateSolutionBrowserAdditionalInformationValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }
    }
}
