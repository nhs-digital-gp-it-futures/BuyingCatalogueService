using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class PublicCloudHostingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublicCloudHostingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/hosting-type-public-cloud")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPublicCloudHosting([FromRoute] [Required] string id)
        {
            var hosting =
                await _mediator.Send(new GetHostingBySolutionIdQuery(id)).ConfigureAwait(false);

            return Ok(new GetPublicCloudResult(hosting?.PublicCloud));
        }
    }
}
