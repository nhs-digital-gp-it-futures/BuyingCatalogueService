using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class RoadMapController : ControllerBase
    {
        private readonly IMediator mediator;

        public RoadMapController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the road map section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the road map section of a solution.</returns>
        [HttpGet] // ReSharper disable once StringLiteralTypo (published endpoint for road map)
        [Route("{id}/sections/roadmap")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([Required] string id)
        {
            var roadMap = await mediator.Send(new GetRoadMapBySolutionIdQuery(id));
            return Ok(new RoadMapResult(roadMap?.Summary));
        }

        /// <summary>
        /// Updates the road map details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of the road map.</param>
        /// <returns>A task representing an operation to update the details of the road map section.</returns>
        [HttpPut] // ReSharper disable once StringLiteralTypo
        [Route("{id}/sections/roadmap")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdateRoadmapViewModel model) =>
            (await mediator.Send(new UpdateRoadmapCommand(id, model?.Summary))).ToActionResult();
    }
}
