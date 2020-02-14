using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class VerifyEpicsResult : ISimpleResult
    {
        public bool IsValid => !ValidEpicsList.Any();

        public HashSet<string> ValidEpicsList { get; } = new HashSet<string>();
        
        public Dictionary<string, string> ToDictionary() =>
            ValidEpicsList.ToConstantValueDictionary(ValidationConstants.Epics);
    }
}
