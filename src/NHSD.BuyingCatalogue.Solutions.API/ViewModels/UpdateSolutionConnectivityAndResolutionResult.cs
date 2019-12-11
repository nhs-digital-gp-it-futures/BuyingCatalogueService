using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionConnectivityAndResolutionResult
    {
        internal UpdateSolutionConnectivityAndResolutionResult(UpdateSolutionConnectivityAndResolutionValidationResult validation)
        {
            Required = validation.ThrowIfNull().Required;
        }

        public HashSet<string> Required { get; }
    }
}
