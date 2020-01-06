using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopHardwareRequirementsController : ControllerBase
    {
        [HttpGet]
        [Route("{id}/sections/native-desktop-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetHardwareRequirements([FromRoute][Required] string id)
        {
            var result = new GetNativeDesktopHardwareRequirementsResult(
                CannedData.ContainsKey(id) ? CannedData[id] : null
                );
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public static ActionResult UpdateHardwareRequirements([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeDesktopHardwareRequirementsViewModel viewModel)
        {
            CannedData[id] = viewModel?.HardwareRequirements;
            return new NoContentResult();
        }
        private static readonly Dictionary<string, string> CannedData = new Dictionary<string, string>();
    }
}
