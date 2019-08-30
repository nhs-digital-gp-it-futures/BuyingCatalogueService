using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides the endpoint for the summary of <see cref="Solution"/> information.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class SolutionsSummaryController : ControllerBase
    {
        /// <summary>
        /// The mediator to pass commands and queries through.
        /// </summary>
        private IMediator Mediator { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionsSummaryController"/> class.
        /// </summary>
        public SolutionsSummaryController(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets a list of solutions that includes information about the organisation and the associated capabilities.
        /// </summary>
        /// <returns>A result containing a list of solutions that includes information about the organisation and the associated capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetAllSolutionSummariesResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GetAllSolutionSummariesResult>> GetAllAsync()
        {
            return Ok(await Mediator.Send(new GetAllSolutionSummariesQuery()));
        }

        /// <summary>
        /// Find a list of Solutions that match the specified list of Capabilities.
        /// </summary>
        /// <param name="filter">The filter criteria to apply to the list of Solutions.</param>
        /// <returns>A task representing an operation to retrieve a list of Solutions that match a set of Capabilities.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetAllSolutionSummariesResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GetAllSolutionSummariesResult>> FindAsync([FromBody][Required]GetAllSolutionSummariesFilter filter)
        {
            return Ok(await Mediator.Send(new GetAllSolutionSummariesQuery(filter)));
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
            var result = await Mediator.Send(new GetSolutionByIdQuery(id));
            return result.Solution == null ? (ActionResult)new NotFoundResult() : Ok(result);
        }
    }
}
