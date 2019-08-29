using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(GetAllSolutionSummariesQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GetAllSolutionSummariesQueryResult>> GetAllAsync()
        {
            return Ok(await Mediator.Send(new GetAllSolutionSummariesQuery()));
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
