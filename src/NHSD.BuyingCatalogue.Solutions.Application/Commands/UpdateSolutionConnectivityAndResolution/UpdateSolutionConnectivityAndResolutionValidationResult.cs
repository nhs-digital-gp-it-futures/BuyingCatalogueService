using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    public class UpdateSolutionConnectivityAndResolutionValidationResult
    {
        public bool IsValid => !MaxLength.Any() && !Required.Any();

        public HashSet<string> MaxLength { get; } = new HashSet<string>();
        
        public HashSet<string> Required { get; } = new HashSet<string>();
    }
}
