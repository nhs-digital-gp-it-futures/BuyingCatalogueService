using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures
{
    public class UpdateSolutionFeaturesValidationResult
    {
        public HashSet<string> MaxLength { get; } = new HashSet<string>();

        public bool IsValid => !MaxLength.Any();
    }
}
