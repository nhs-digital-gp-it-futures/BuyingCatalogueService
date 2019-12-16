using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class RequiredValidator
    {
        private readonly RequiredResult _requiredResult;

        internal RequiredValidator() => _requiredResult = new RequiredResult();

        internal RequiredValidator Validate(string field, string fieldLabel)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                _requiredResult.Required.Add(fieldLabel);
            }
            return this;
        }

        internal RequiredValidator Validate(IEnumerable<string> fields, string fieldLabel)
        {
            if (!fields.Any())
            {
                _requiredResult.Required.Add(fieldLabel);
            }
            return this;
        }

        internal RequiredResult Result() => _requiredResult;
    }
}
