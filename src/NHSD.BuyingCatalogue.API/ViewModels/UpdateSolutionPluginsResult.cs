using System.Collections.Generic;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins;

namespace NHSD.BuyingCatalogue.API.ViewModels
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
