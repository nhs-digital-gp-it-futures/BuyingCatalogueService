using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionNativeMobileFirstResult
    {
        internal UpdateSolutionNativeMobileFirstResult(RequiredResult validationResult)
            => Required = validationResult.Required;

        public HashSet<string> Required { get; }
    }
}
