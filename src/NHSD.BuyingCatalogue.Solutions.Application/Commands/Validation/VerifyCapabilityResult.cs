using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class VerifyCapabilityResult : ISimpleResult
    {
        public bool IsValid => !ValidCapabilityList.Any();

        public HashSet<string> ValidCapabilityList { get; } = new HashSet<string>();

        public Dictionary<string, string> ToDictionary() =>
            ValidCapabilityList.ToConstantValueDictionary(ValidationConstants.Capability);
    }
}
