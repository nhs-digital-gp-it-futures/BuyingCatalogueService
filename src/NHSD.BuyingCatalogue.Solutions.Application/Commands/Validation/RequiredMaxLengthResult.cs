using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class RequiredMaxLengthResult : ISimpleResult
    {
        private readonly RequiredResult requiredResult;
        private readonly MaxLengthResult maxLengthResult;

        public RequiredMaxLengthResult(RequiredResult requiredResult = null, MaxLengthResult maxLengthResult = null)
        {
            this.requiredResult = requiredResult ?? new RequiredResult();
            this.maxLengthResult = maxLengthResult ?? new MaxLengthResult();
        }

        public HashSet<string> Required => requiredResult.Required;

        public HashSet<string> MaxLength => maxLengthResult.MaxLength;

        public bool IsValid => requiredResult.IsValid && maxLengthResult.IsValid;

        public Dictionary<string, string> ToDictionary() => new List<Dictionary<string, string>>
            {
                Required.ToConstantValueDictionary(ValidationConstants.Required),
                MaxLength.ToConstantValueDictionary(ValidationConstants.MaxLength),
            }
            .Combine();
    }
}
