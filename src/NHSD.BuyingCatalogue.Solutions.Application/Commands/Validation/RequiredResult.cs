using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class RequiredResult : ISimpleResult
    {
        public bool IsValid => !Required.Any();

        public HashSet<string> Required { get; } = new();

        public Dictionary<string, string> ToDictionary() => Required.ToConstantValueDictionary(ValidationConstants.Required);
    }
}
