using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Epics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Epics;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class EpicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EpicsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/epics")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAll([FromRoute][Required]string id)
        {
            var result = new EpicsResult() { SolutionId = id };

            return Ok(result);
        }

       // <summary>
       // Updates the solution's epics.
       // </summary>
       // <param name="id"></param>
       // <param name="viewModel"></param>
       // <returns>A Task representing the operation to update the details of the epics section</returns>
      [HttpPut]
      [Route("{id}/sections/epics")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<ActionResult> UpdateAsync([FromRoute] [Required] string id,
          [FromBody] [Required] UpdateEpicsViewModel viewModel) =>
          (await _mediator.Send(new UpdateClaimedEpicsCommand(id, viewModel?.ClaimedEpics))
              .ConfigureAwait(false)).ToActionResult();
    }
}
