using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class RequiredValidator
    {
        private readonly RequiredResult requiredResult;

        internal RequiredValidator() => requiredResult = new RequiredResult();

        internal RequiredValidator Validate(string field, string fieldLabel)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                requiredResult.Required.Add(fieldLabel);
            }

            return this;
        }

        internal RequiredValidator Validate(IEnumerable<string> fields, string fieldLabel)
        {
            if (fields.All(string.IsNullOrWhiteSpace))
            {
                requiredResult.Required.Add(fieldLabel);
            }

            return this;
        }

        internal RequiredResult Result() => requiredResult;
    }
}
