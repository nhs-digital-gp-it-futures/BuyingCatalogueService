using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides a set of endpoints for the information related to the entity <see cref="Capability"/>.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class CapabilitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _context;

        public CapabilitiesController(
            IMediator mediator,
            IHttpContextAccessor context)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets a list of capabilitiies.
        /// </summary>
        /// <returns>A result containing a list of capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ListCapabilitiesResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListCapabilitiesResult>> ListAsync()
        {
            return Ok(await _mediator.Send(new ListCapabilitiesQuery(_context)));
        }
    }
}
