using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Capabilities.API.ViewModels;
using NHSD.BuyingCatalogue.Capabilities.Contracts;

namespace NHSD.BuyingCatalogue.Capabilities.API
{
    /// <summary>
    /// Provides a set of endpoints for the information related to the capability entity.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [AllowAnonymous]
    public sealed class CapabilitiesController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilitiesController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        public CapabilitiesController(IMediator mediator) => this.mediator = mediator;

        /// <summary>
        /// Gets a list of capabilities.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListCapabilitiesResult>> ListAsync() => Ok(
            new ListCapabilitiesResult(await mediator.Send(new ListCapabilitiesQuery())));
    }
}
