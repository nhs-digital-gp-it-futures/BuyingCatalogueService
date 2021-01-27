using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hosting;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.PrivateCloud;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Hosting
{
    /// <summary>
    /// Provides an endpoint to manage the private cloud hosting details for a given solution.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class PrivateCloudHostingController : ControllerBase
    {
        private readonly IMediator mediator;

        public PrivateCloudHostingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/hosting-type-private-cloud")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([Required] string id)
        {
            var hosting = await mediator.Send(new GetHostingBySolutionIdQuery(id));

            return Ok(new GetPrivateCloudResult(hosting?.PrivateCloud));
        }

        [HttpPut]
        [Route("{id}/sections/hosting-type-private-cloud")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdatePrivateCloudViewModel model) =>
            (await mediator.Send(new UpdatePrivateCloudCommand(id, model))).ToActionResult();
    }
}
