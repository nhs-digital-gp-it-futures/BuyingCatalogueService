using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    internal class UpdateFormRequiredMaxLengthResult
    {
        internal UpdateFormRequiredMaxLengthResult(RequiredMaxLengthResult validationResult)
        {
            Required = validationResult?.Required;
            MaxLength = validationResult?.MaxLength;
        }

        public HashSet<string> Required { get; }

        public HashSet<string> MaxLength { get; }
    }
}
