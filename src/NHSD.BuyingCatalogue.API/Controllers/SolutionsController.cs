using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Infrastructure;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides the endpoint for the summary of <see cref="Solution"/> information.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(
        Roles = Roles.Authority + "," + Roles.Buyer + "," + Roles.Public + "," + Roles.Supplier,
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public sealed class SolutionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityProvider _idProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionsController"/> class.
        /// </summary>
        public SolutionsController(
            IMediator mediator,
            IIdentityProvider idProvider)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _idProvider = idProvider ?? throw new ArgumentNullException(nameof(idProvider));
        }

        /// <summary>
        /// Gets a list of solutions that includes information about the organisation and the associated capabilities.
        /// </summary>
        /// <returns>A result containing a list of solutions that includes information about the organisation and the associated capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ListSolutionsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListAsync()
        {
            return Ok(await _mediator.Send(new ListSolutionsQuery(_idProvider)));
        }

        /// <summary>
        /// Find a list of Solutions that match the specified list of Capabilities.
        /// </summary>
        /// <param name="filter">The filter criteria to apply to the list of Solutions.</param>
        /// <returns>A task representing an operation to retrieve a list of Solutions that match a set of Capabilities.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ListSolutionsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListByFilterAsync([FromBody][Required]ListSolutionsFilter filter)
        {
            return Ok(await _mediator.Send(new ListSolutionsQuery(_idProvider, filter)));
        }

        /// <summary>
        /// Get a solution with specified unique identifier
        /// </summary>
        /// <param name="id">unique identifier of solution</param>
        /// <returns>Solution with specified unique identifier</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(GetSolutionByIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<GetSolutionByIdResult>> ById([FromRoute][Required]string id)
        {
            GetSolutionByIdResult result = await _mediator.Send(new GetSolutionByIdQuery(_idProvider, id));
            return result.Solution == null ? (ActionResult)new NotFoundResult() : Ok(result);
        }
    }
}
