using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class MaxLengthResult : ISimpleResult
    {
        public HashSet<string> MaxLength { get; } = new();

        public bool IsValid => !MaxLength.Any();

        public Dictionary<string, string> ToDictionary() => MaxLength.ToConstantValueDictionary(ValidationConstants.MaxLength);
    }
}
