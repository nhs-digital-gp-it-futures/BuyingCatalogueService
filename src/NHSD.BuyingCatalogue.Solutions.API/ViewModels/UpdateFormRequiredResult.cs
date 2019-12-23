using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    internal class UpdateFormRequiredResult
    {
        internal UpdateFormRequiredResult(RequiredResult validationResult) => Required = validationResult?.Required;

        public HashSet<string> Required { get; }
    }
}
