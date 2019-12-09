using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class HardwareRequirementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HardwareRequirementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/browser-hardware-requirements")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetHardwareRequirementsAsync([FromRoute][Required]string id)
        {
            var result = new GetHardwareRequirementsResult()
            {
                HardwareRequirements = "Hardware Info for solution " + id
            };

            return Ok(result);
            //var solution = await _mediator.Send(new GetSolutionByIdQuery(id)).ConfigureAwait(false);
            //return solution == null ? (ActionResult)new NotFoundResult() : Ok(new GetHardwareRequirementsResult());
        }
    }
}
