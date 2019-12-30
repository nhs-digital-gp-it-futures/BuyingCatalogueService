using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class ValidationResultExtensions
    {
        internal static ActionResult ToActionResult(this MaxLengthResult validationResult) => validationResult.ToActionResult(r => r.ToDictionary());

        internal static ActionResult ToActionResult(this RequiredResult validationResult) => validationResult.ToActionResult(r => r.ToDictionary());

        internal static ActionResult ToActionResult(this RequiredMaxLengthResult validationResult) => validationResult.ToActionResult(r => r.ToDictionary());

        private static ActionResult ToActionResult<TResult>(this TResult validationResult, Func<TResult, Dictionary<string, string>> toDictionary) where TResult : IResult =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(toDictionary(validationResult));
    }
}
