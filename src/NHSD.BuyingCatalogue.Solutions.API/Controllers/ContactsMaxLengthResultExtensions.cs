using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class ContactsMaxLengthResultExtensions
    {
        internal static ActionResult ToActionResult(this ContactsMaxLengthResult validationResult) =>
            validationResult.ToActionResult(r => r.ToDictionary());
    }
}
