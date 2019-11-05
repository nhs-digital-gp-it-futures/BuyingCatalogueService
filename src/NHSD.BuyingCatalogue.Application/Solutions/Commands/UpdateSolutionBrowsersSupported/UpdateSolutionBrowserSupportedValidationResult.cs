using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported
{
    public sealed class UpdateSolutionBrowserSupportedValidationResult
    {
        public HashSet<string> Required { get; } = new HashSet<string>();

        public bool IsValid => !Required.Any();
    }
}
