using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class VerifyCapabilityResult : ISimpleResult
    {
        public bool IsValid => !CapabilityValid.Any();

        public HashSet<string> CapabilityValid { get; } = new HashSet<string>();

        public Dictionary<string, string> ToDictionary() =>
            CapabilityValid.ToConstantValueDictionary(ValidationConstants.Capability);
    }
}
