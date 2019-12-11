using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    public sealed class UpdateSolutionBrowserAdditionalInformationValidationResult
    {
        public HashSet<string> MaxLength { get; } = new HashSet<string>();

        public bool IsValid => !MaxLength.Any();
    }
}
