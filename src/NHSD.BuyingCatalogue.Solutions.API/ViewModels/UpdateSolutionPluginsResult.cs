using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionPluginsResult
    {
        public UpdateSolutionPluginsResult(UpdateSolutionPluginsValidationResult updateSolutionPluginsValidationResult)
        {
            Required = updateSolutionPluginsValidationResult.Required;
            MaxLength = updateSolutionPluginsValidationResult.MaxLength;
        }

        public HashSet<string> Required { get; }

        public HashSet<string> MaxLength { get; }
    }
}
