using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;

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
        [Route("{id}/sections/capabilities")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult Get([FromRoute][Required]string id)
        {
            // Canned Data
            var result = new CapabilitiesResult
            {
                SolutionId = id
            };
            return Ok(result);
        }
    }
}
