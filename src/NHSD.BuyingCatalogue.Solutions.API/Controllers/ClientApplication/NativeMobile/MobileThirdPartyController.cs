using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeMobile
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class MobileThirdPartyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MobileThirdPartyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/native-mobile-third-party")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetNativeMobileThirdParty([FromRoute] [Required] string id)
        {
            var clientApplication =
                await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new GetMobileThirdPartyResult(clientApplication?.MobileThirdParty));
        }

        [HttpPut]
        [Route("{id}/sections/native-mobile-third-party")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateNativeMobileThirdParty([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeMobileThirdPartyViewModel viewModel) =>
            (await _mediator.Send(new UpdateSolutionMobileThirdPartyCommand(id, viewModel)).ConfigureAwait(false))
                .ToActionResult();
    }
}
