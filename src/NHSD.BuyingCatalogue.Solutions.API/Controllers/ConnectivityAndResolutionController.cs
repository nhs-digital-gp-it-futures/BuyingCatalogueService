using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class ConnectivityAndResolutionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConnectivityAndResolutionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/connectivity-and-resolution")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetHardwareRequirementsAsync([FromRoute][Required]string id)
        {
            var result = new GetConnectivityAndResolutionResult
            {
                MinimumConnectionSpeed = CannedData.Keys.Contains(id) ? CannedData[id].Item1 : null,
                MinimumDesktopResolution = CannedData.Keys.Contains(id) ? CannedData[id].Item2 : null,
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/connectivity-and-resolution")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateConnectivityAndResolutionAsync([FromRoute][Required]string id, [FromBody][Required] UpdateSolutionConnectivityAndResolutionViewModel viewModel)
        {
            CannedData[id] = (viewModel.ThrowIfNull().MinimumConnectionSpeed, viewModel.ThrowIfNull().MinimumDesktopResolution);

            var validationResult = await _mediator.Send(new UpdateSolutionConnectivityAndResolutionCommand(id, viewModel)).ConfigureAwait(false);

            return validationResult.IsValid ? (ActionResult)new NoContentResult()
                : BadRequest(new UpdateSolutionConnectivityAndResolutionResult(validationResult));
        }

        //canned data
        private static readonly Dictionary<string, (string, string)> CannedData = new Dictionary<string, (string, string)>();
    }
}
