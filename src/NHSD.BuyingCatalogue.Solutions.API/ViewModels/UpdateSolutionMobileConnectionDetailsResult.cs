using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionMobileConnectionDetailsResult
    {
        public UpdateSolutionMobileConnectionDetailsResult(MaxLengthResult result)
        {
            MaxLength = result.ThrowIfNull().MaxLength;
        }

        public HashSet<string> MaxLength { get; }
    }
}
