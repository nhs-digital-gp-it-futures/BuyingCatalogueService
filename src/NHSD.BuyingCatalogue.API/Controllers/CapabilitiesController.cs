using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides a set of endpoints for the information related to the capability entity.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class CapabilitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="CapabilitiesController"/> class.
        /// </summary>
        public CapabilitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a list of capabilities.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ListCapabilitiesResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListCapabilitiesResult>> ListAsync()
        {
            return Ok(await _mediator.Send(new ListCapabilitiesQuery()));
        }
    }
}
