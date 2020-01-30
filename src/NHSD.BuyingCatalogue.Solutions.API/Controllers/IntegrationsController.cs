using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class IntegrationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IntegrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the integrations section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the integrations section of a solution.</returns>

        [HttpGet]
        [Route("{id}/sections/integrations")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> Get([FromRoute][Required]string id)
        {
            var integrations = await _mediator.Send(new GetIntegrationsBySolutionIdQuery(id)).ConfigureAwait(false);
            return Ok(new IntegrationsResult(integrations?.Url));
        }
    }
}
