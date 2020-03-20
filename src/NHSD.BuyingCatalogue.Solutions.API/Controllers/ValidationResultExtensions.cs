using System;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class ValidationResultExtensions
    {
        internal static ActionResult ToActionResult<TResult>(this TResult validationResult) where TResult : ISimpleResult  =>
            validationResult.ToActionResult(r => r.ToDictionary());

        internal static ActionResult ToActionResult<TResult>(this TResult validationResult, Func<TResult, object> ToError) where TResult : IResult =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(ToError(validationResult));
    }
}
