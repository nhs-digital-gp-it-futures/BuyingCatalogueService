using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class ValidationResultMappingExtensions
    {
        private const string Required = "required";
        private const string MaxLength = "maxLength";

        internal static Dictionary<string, string> ToDictionary(this MaxLengthResult validationResult) => validationResult.MaxLength.ToConstantValueDictionary(MaxLength);

        internal static Dictionary<string, string> ToDictionary(this RequiredResult validationResult) => validationResult.Required.ToConstantValueDictionary(Required);

        internal static Dictionary<string, string> ToDictionary(this RequiredMaxLengthResult validationResult) =>
            new List<Dictionary<string, string>>
                {
                    validationResult.Required.ToConstantValueDictionary(Required),
                    validationResult.MaxLength.ToConstantValueDictionary(MaxLength)
                }
                .Combine();
    }
}
