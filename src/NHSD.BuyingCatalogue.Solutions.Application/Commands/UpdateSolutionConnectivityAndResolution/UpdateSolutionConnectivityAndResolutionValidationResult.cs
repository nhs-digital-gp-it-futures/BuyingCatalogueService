using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    public class UpdateSolutionConnectivityAndResolutionValidationResult
    {
        public bool IsValid => !Required.Any();
        
        public HashSet<string> Required { get; } = new HashSet<string>();
    }
}
