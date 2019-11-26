using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures
{
    public class UpdateSolutionFeaturesValidationResult
    {
        public HashSet<string> MaxLength { get; } = new HashSet<string>();

        public bool IsValid => !MaxLength.Any();
    }
}
