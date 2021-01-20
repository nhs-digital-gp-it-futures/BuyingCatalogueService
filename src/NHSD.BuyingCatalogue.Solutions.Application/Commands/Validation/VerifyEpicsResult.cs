using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class VerifyEpicsResult : ISimpleResult
    {
        public bool IsValid => !InvalidEpicsList.Any();

        public HashSet<string> InvalidEpicsList { get; } = new();

        public Dictionary<string, string> ToDictionary() =>
            InvalidEpicsList.ToConstantValueDictionary(ValidationConstants.InvalidEpics);
    }
}
