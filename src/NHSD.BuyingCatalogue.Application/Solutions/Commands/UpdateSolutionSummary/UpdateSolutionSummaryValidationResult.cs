using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary
{
    public class UpdateSolutionSummaryValidationResult
    {
        public HashSet<string> Required { get; } = new HashSet<string>();

        public HashSet<string> MaxLength { get; } = new HashSet<string>();

        public bool IsValid => !(Required.Any() || MaxLength.Any());
    }
}
