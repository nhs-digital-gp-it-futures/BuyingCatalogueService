using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class CapabilitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CapabilitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/sections/capabilities")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAll()
        {
            return Ok();
        }

        /// <summary>
        /// Updates the solution's capabilities.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>A Task representing the operation to update the details of the capabilities section</returns>
        [HttpPut]
        [Route("{id}/sections/capabilities")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateCapabilitiesViewModel viewModel) =>
            (await _mediator.Send(new UpdateCapabilitiesCommand(id, viewModel?.NewCapabilitiesReferences))
                .ConfigureAwait(false)).ToActionResult();
    }
}
