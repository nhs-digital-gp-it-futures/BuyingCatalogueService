using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.API.ViewModels;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowsersSupportedController : ControllerBase
    {
        /// <summary>
        /// Updates the browsers supported of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="browsersSupportedRequest">The details of the supported browsers.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpPut]
        [Route("{id}/sections/browsers-supported")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateBrowsersSupportedAsync([FromRoute][Required]string id, [FromBody][Required]BrowsersSupportedRequest browsersSupportedRequest)
        {
            BrowsersSupported = browsersSupportedRequest.BrowsersSupported.ToList();

            MobileResponsive = browsersSupportedRequest.MobileResponsive;

            return NoContent();
        }

        /// <summary>
        /// Gets the browsers supported for the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/browsers-supported")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBrowsersSupportedAsync([FromRoute][Required]string id)
        {
            //Canned Data
            return Ok(new BrowsersSupportedResult
            {
                BrowsersSupported = BrowsersSupported, MobileResponsive = MobileResponsive
            });
        }

        private static List<string> BrowsersSupported = new List<string>();

        private static string MobileResponsive = "yes";
    }
}
