using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hosting;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.HybridHostingType;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hosting
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class HybridHostingTypeController : ControllerBase
    {
        private readonly IMediator mediator;

        public HybridHostingTypeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/hosting-type-hybrid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([Required] string id)
        {
            var hosting = await mediator.Send(new GetHostingBySolutionIdQuery(id));
            return Ok(new GetHybridHostingTypeResult(hosting?.HybridHostingType));
        }

        [HttpPut]
        [Route("{id}/sections/hosting-type-hybrid")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdateHybridHostingTypeViewModel model) =>
            (await mediator.Send(new UpdateHybridHostingTypeCommand(id, model))).ToActionResult();
    }
}
