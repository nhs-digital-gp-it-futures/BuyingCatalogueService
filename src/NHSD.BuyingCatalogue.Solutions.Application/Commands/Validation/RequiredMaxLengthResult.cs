using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class RequiredMaxLengthResult : ISimpleResult
    {
        private readonly RequiredResult _requiredResult;
        private readonly MaxLengthResult _maxLengthResult;

        public RequiredMaxLengthResult(RequiredResult requiredResult = null, MaxLengthResult maxLengthResult = null)
        {
            _requiredResult = requiredResult ?? new RequiredResult();
            _maxLengthResult = maxLengthResult ?? new MaxLengthResult();
        }

        public HashSet<string> Required => _requiredResult.Required;

        public HashSet<string> MaxLength => _maxLengthResult.MaxLength;

        public bool IsValid => _requiredResult.IsValid && _maxLengthResult.IsValid;

        public Dictionary<string, string> ToDictionary() =>
            new List<Dictionary<string, string>>
            {
                Required.ToConstantValueDictionary(ValidationConstants.Required),
                MaxLength.ToConstantValueDictionary(ValidationConstants.MaxLength),
            }
            .Combine();
    }
}
