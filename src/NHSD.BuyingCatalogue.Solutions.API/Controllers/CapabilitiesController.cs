using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class CapabilitiesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CapabilitiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/capabilities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAll([Required] string id)
        {
            var result = new CapabilitiesResult { SolutionId = id };

            return Ok(result);
        }

        /// <summary>
        /// Updates the solution's capabilities.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of the supported capabilities.</param>
        /// <returns>A <see cref="Task"/> representing the operation to update the details of the capabilities section.</returns>
        [HttpPut]
        [Route("{id}/sections/capabilities")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdateCapabilitiesViewModel model) =>
            (await mediator.Send(new UpdateCapabilitiesCommand(id, model?.NewCapabilitiesReferences))).ToActionResult();
    }
}
