using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    internal class UpdateFormMaxLengthResult
    {
        internal UpdateFormMaxLengthResult(MaxLengthResult validationResult) => MaxLength = validationResult?.MaxLength;

        public HashSet<string> MaxLength { get; }
    }
}
