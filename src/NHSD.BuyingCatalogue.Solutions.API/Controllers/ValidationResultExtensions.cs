using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class ValidationResultExtensions
    {
        private const string Required = "required";
        private const string MaxLength = "maxLength";

        internal static ActionResult ToActionResult(this MaxLengthResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(validationResult.ToFieldListValidation());

        internal static ActionResult ToActionResult(this RequiredResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(validationResult.ToFieldListValidation());

        internal static ActionResult ToFieldListValidationActionResult(this RequiredMaxLengthResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(validationResult.ToFieldListValidation());

        internal static Dictionary<string, string> ToFieldListValidation(this MaxLengthResult validationResult) =>
            validationResult.MaxLength.ToDictionary(k => k, v => MaxLength);

        internal static Dictionary<string, string> ToFieldListValidation(this RequiredResult validationResult) =>
            validationResult.Required.ToDictionary(k => k, v => Required);

        internal static Dictionary<string, string> ToFieldListValidation(this RequiredMaxLengthResult validationResult) =>
            new List<Dictionary<string, string>>
                {
                    validationResult.Required.ToDictionary(k => k, v => Required),
                    validationResult.MaxLength.ToDictionary(k => k, v => MaxLength)
                }
                .SelectMany(dict => dict)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
