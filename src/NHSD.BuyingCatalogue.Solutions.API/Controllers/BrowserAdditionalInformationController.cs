using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowserAdditionalInformationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrowserAdditionalInformationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/browser-additional-information")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetAdditionalInformationAsync([FromRoute] [Required] string id)
        {
            var result = new GetBrowserAdditionalInformationResult
            {
                AdditionalInformation = CannedData.Keys.Contains(id) ? CannedData[id] : null
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/browser-additional-information")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult UpdateAdditionalInformationAsync([FromRoute] [Required] string id,
            [FromBody] [Required]
            UpdateSolutionBrowserAdditionalInformationViewModel viewModel)
        {
            CannedData[id] = (viewModel.ThrowIfNull().AdditionalInformation);
            return NoContent();
        }

        //canned data
        private static readonly Dictionary<string, string> CannedData = new Dictionary<string, string>();
    }
}
