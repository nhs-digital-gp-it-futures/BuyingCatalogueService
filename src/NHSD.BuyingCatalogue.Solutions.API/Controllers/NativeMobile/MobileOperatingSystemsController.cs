using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
        [Route("api/v1/solutions")]
        [ApiController]
        [Produces("application/json")]
        [AllowAnonymous]
        public class MobileOperatingSystemsController : ControllerBase
        {
            private readonly IMediator _mediator;

            public MobileOperatingSystemsController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpGet]
            [Route("{id}/sections/native-mobile-operating-systems")]
            [ProducesResponseType((int)HttpStatusCode.BadRequest)]
            [ProducesResponseType((int)HttpStatusCode.NoContent)]
            [ProducesResponseType((int)HttpStatusCode.NotFound)]
            public async Task<ActionResult> GetMobileOperatingSystems([FromRoute] [Required] string id)
            {
                var clientApplication = await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id))
                    .ConfigureAwait(false);

                return Ok(new GetMobileOperatingSystemsResult(clientApplication?.MobileOperatingSystems));
            }

            [HttpPut]
            [Route("{id}/sections/native-mobile-operating-systems")]
            [ProducesResponseType((int)HttpStatusCode.BadRequest)]
            [ProducesResponseType((int)HttpStatusCode.NoContent)]
            [ProducesResponseType((int)HttpStatusCode.NotFound)]
            public async Task<ActionResult> UpdateMobileOperatingSystems([FromRoute] [Required] string id,
                [FromBody] [Required] UpdateSolutionMobileOperatingSystemsViewModel viewModel) =>
                (await _mediator.Send(new UpdateSolutionMobileOperatingSystemsCommand(id, viewModel)).ConfigureAwait(false)).ToActionResult();
        }
}
