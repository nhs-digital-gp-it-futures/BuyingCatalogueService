using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class RoadmapController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoadmapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the roadmap section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the roadmap section of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/roadmap")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get([FromRoute][Required]string id)
        {
            var roadMap = await _mediator.Send(new GetRoadMapBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new RoadMapResult(roadMap?.Summary));
        }


        /// <summary>
        /// Updates the roadmap details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="viewModel">The details of the roadmap.</param>
        /// <returns>A task representing an operation to update the details of the roadmap section.</returns>
        [HttpPut]
        [Route("{id}/sections/roadmap")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update([FromRoute][Required]string id, [FromBody][Required]UpdateRoadmapViewModel viewModel) =>
            (await _mediator.Send(new UpdateRoadmapCommand(id, viewModel?.Summary)).ConfigureAwait(false)).ToActionResult();
    }
}
