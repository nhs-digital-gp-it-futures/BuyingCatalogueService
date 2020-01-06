using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileConnectionDetails;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeMobile
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
        [Route("{id}/sections/native-mobile-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetMobileConnectionDetails([FromRoute] [Required] string id)
        {
            var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id))
                .ConfigureAwait(false);

            return Ok(new GetMobileConnectionDetailsResult(clientApplication?.MobileConnectionDetails));
        }

        [HttpPut]
        [Route("{id}/sections/native-mobile-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateMobileConnectionDetails([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateSolutionMobileConnectionDetailsViewModel viewModel) =>
            (await _mediator.Send(new UpdateSolutionMobileConnectionDetailsCommand(id, viewModel)).ConfigureAwait(false)).ToActionResult();
    }
}
