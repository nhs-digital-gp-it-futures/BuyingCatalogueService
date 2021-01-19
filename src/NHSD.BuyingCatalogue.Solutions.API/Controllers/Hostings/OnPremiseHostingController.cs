using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class OnPremiseHostingController : ControllerBase
    {
        private readonly IMediator mediator;

        public OnPremiseHostingController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/hosting-type-on-premise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([Required] string id)
        {
            var hosting = await mediator.Send(new GetHostingBySolutionIdQuery(id));

            return Ok(new GetOnPremiseResult(hosting?.OnPremise));
        }

        [HttpPut]
        [Route("{id}/sections/hosting-type-on-premise")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdateOnPremiseViewModel model) =>
            (await mediator.Send(new UpdateOnPremiseCommand(id, model))).ToActionResult();
    }
}
