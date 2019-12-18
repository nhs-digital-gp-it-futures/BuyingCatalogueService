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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class MobileConnectionDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MobileConnectionDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/mobile-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetMobileConnectionDetails([FromRoute] [Required] string id)
        {
            var result = new GetMobileConnectionDetailsResult()
            {
                MinimumConnectionSpeed = CannedData.Keys.Contains(id) ? CannedData[id].Item1 : null,
                ConnectionType = CannedData.Keys.Contains(id) ? CannedData[id].Item2 : null,
                ConnectionRequirementsDescription = CannedData.Keys.Contains(id) ? CannedData[id].Item3 : null
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/mobile-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateMobileConnectionDetails([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateSolutionMobileConnectionDetailsViewModel viewModel)
        {
            CannedData[id] = (viewModel.ThrowIfNull().MinimumConnectionSpeed, viewModel.ThrowIfNull().ConnectionType,
                viewModel.ThrowIfNull().ConnectionRequirementsDescription);
            var command = new UpdateSolutionMobileConnectionDetailsCommand(id, viewModel);
            var validationResult = await _mediator.Send(command).ConfigureAwait(false);

            return validationResult.IsValid ? (ActionResult)new NoContentResult()
                : BadRequest(validationResult);
        }

        //Canned Data
        private static readonly Dictionary<string, (string, IEnumerable<string>, string)> CannedData = new Dictionary<string, (string, IEnumerable<string>, string)>();
    }
}
