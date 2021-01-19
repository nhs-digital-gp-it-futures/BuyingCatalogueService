using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Epics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class EpicsController : ControllerBase
    {
        private readonly IMediator mediator;

        public EpicsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/epics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAll([Required] string id)
        {
            var result = new EpicsResult { SolutionId = id };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/epics")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAsync([Required] string id, UpdateEpicsViewModel model) =>
            (await mediator.Send(new UpdateClaimedEpicsCommand(id, model?.ClaimedEpics))).ToActionResult();
    }
}
