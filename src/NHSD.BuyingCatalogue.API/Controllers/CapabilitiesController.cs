using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;
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
        private readonly IIdentityProvider _idProvider;

        public CapabilitiesController(
            IMediator mediator,
            IIdentityProvider idProvider)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _idProvider = idProvider ?? throw new ArgumentNullException(nameof(idProvider));
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
            return Ok(await _mediator.Send(new ListCapabilitiesQuery(_idProvider)));
        }
    }
}
