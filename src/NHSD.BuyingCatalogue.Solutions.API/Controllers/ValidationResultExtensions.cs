using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class ValidationResultExtensions
    {
        internal static ActionResult ToActionResult(this RequiredMaxLengthResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(new UpdateFormRequiredMaxLengthResult(validationResult));

        internal static ActionResult ToActionResult(this MaxLengthResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(new UpdateFormMaxLengthResult(validationResult));

        internal static ActionResult ToActionResult(this RequiredResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(new UpdateFormRequiredResult(validationResult));

        internal static ActionResult ToFieldListValidationActionResult(this RequiredMaxLengthResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(validationResult.ToFieldListValidation());

        internal static Dictionary<string, string> ToFieldListValidation(this MaxLengthResult validationResult) =>
            validationResult.MaxLength.ToDictionary(k => k, v => "maxLength");

        internal static Dictionary<string, string> ToFieldListValidation(this RequiredResult validationResult) =>
            validationResult.Required.ToDictionary(k => k, v => "required");

        internal static Dictionary<string, string> ToFieldListValidation(this RequiredMaxLengthResult validationResult) =>
            new List<Dictionary<string, string>>
                {
                    validationResult.Required.ToDictionary(k => k, v => "required"),
                    validationResult.MaxLength.ToDictionary(k => k, v => "maxLength")
                }
                .SelectMany(dict => dict)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
