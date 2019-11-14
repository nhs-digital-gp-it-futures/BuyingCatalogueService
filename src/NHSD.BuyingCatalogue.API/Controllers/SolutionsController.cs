using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.API.ViewModels.Preview;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides the endpoint to manage a solution.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class SolutionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionsController"/> class.
        /// </summary>
        public SolutionsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets a list of solutions that includes information about the organisation and the associated capabilities.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of solutions that includes information about the organisation and the associated capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ListSolutionsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListAsync()
        {
            return Ok(new ListSolutionsResult(await _mediator.Send(new ListSolutionsQuery())));
        }

        /// <summary>
        /// Gets a list of foundation solutions that includes information about the organisation and the associated capabilities.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of solutions that includes information about the organisation and the associated capabilities.</returns>
        [HttpGet]
        [Route("foundation")]
        [ProducesResponseType(typeof(ListSolutionsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListFoundationAsync()
        {
            return Ok(new ListSolutionsResult(await _mediator.Send(new ListSolutionsQuery())));
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
            return Ok(new ListSolutionsResult(await _mediator.Send(new ListSolutionsQuery(filter))));
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of a Solution.</returns>
        [HttpGet]
        [Route("{id}/Dashboard")]
        [ProducesResponseType(typeof(SolutionDashboardResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<SolutionDashboardResult>> Dashboard([FromRoute][Required]string id)
        {
            var result = await _mediator.Send(new GetSolutionByIdQuery(id));
            return result == null ? (ActionResult)new NotFoundResult() : Ok(new SolutionDashboardResult(result));
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of a Solution.</returns>
        [HttpGet]
        [Route("{id}/Preview")]
        [ProducesResponseType(typeof(SolutionPreviewResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<SolutionPreviewResult>> Preview([FromRoute][Required]string id)
        {
            var result = await _mediator.Send(new GetSolutionByIdQuery(id));
            return result == null ? (ActionResult)new NotFoundResult() : Ok(new SolutionPreviewResult(result));
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of a Solution.</returns>
        [HttpGet]
        [Route("{id}/Public")]
        [ProducesResponseType(typeof(SolutionPreviewResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<SolutionPreviewResult>> Public([FromRoute][Required]string id)
        {
            var result = await _mediator.Send(new GetSolutionByIdQuery(id));
            return result == null ? (ActionResult)new NotFoundResult() : Ok(new SolutionPreviewResult(result));
        }

        /// <summary>
        /// Submits a solution for review.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to update the state of a solution to submitted for review.</returns>
        [HttpPut]
        [Route("{id}/SubmitForReview")]
        [ProducesResponseType(typeof(SubmitSolutionForReviewResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> SubmitForReviewAsync([FromRoute][Required] string id)
        {
            SubmitSolutionForReviewCommandResult result = await _mediator.Send(new SubmitSolutionForReviewCommand(id));
            return result.IsSuccess ? NoContent() : (ActionResult)BadRequest(SubmitSolutionForReviewResult.Create(result.Errors));
        }
    }
}
