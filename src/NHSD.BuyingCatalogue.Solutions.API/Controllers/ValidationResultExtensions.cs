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
    }
}
