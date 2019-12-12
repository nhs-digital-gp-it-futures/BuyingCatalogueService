using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowserMobileFirstController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrowserMobileFirstController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/browser-mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetMobileFirstAsync([FromRoute] [Required] string id)
        {
            if (!CannedData.ContainsKey(id))
            {
                CannedData[id] = null;
            }

            var result = new GetBrowserMobileFirstResult
            {
                MobileFirstDesign = CannedData[id]
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/browser-mobile-first")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult UpdateMobileFirstAsync([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateSolutionBrowserMobileFirstViewModel viewModel)
        {
            CannedData[id] = (viewModel.ThrowIfNull().MobileFirstDesign);
            return NoContent();
        }

        //Canned Data
        private static readonly Dictionary<string, bool?> CannedData = new Dictionary<string, bool?>();
    }
}
