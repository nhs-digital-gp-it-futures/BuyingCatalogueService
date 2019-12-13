using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    public sealed class UpdateSolutionBrowserMobileFirstValidationResult
    {
        public HashSet<string> Required { get; } = new HashSet<string>();

        public bool IsValid => !Required.Any();
    }
}
