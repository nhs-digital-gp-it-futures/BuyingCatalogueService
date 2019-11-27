using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    public sealed class UpdateSolutionClientApplicationTypesValidationResult
    {
        public HashSet<string> Required { get; } = new HashSet<string>();

        public bool IsValid => !Required.Any();
    }
}
