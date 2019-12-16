using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionPluginsResult
    {
        internal UpdateSolutionPluginsResult(RequiredMaxLengthResult updateSolutionPluginsValidationResult)
        {
            Required = updateSolutionPluginsValidationResult.Required;
            MaxLength = updateSolutionPluginsValidationResult.MaxLength;
        }

        public HashSet<string> Required { get; }

        public HashSet<string> MaxLength { get; }
    }
}
