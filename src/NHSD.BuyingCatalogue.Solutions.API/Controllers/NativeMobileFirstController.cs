using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class NativeMobileFirstController : ControllerBase
    {
        //Canned Data
        private static readonly Dictionary<string, string> CannedData = new Dictionary<string, string>();

        public NativeMobileFirstController()
        {
        }

        [HttpGet]
        [Route("{id}/sections/mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetMobileFirstAsync([FromRoute] [Required] string id)
        {
            if (!CannedData.ContainsKey(id))
            {
                CannedData[id] = null;
            }

            var result = new GetNativeMobileFirstResult
            {
                MobileFirstDesign = CannedData[id]
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult UpdateMobileFirstAsync([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateSolutionNativeMobileFirstResult viewModel)
        {
            CannedData[id] = viewModel.ThrowIfNull().MobileFirstDesign;
            return NoContent();
        }
    }
}
