using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    /// <summary>
    /// Provides the endpoint to manage a solution.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class SolutionsController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionsController"/> class.
        /// </summary>
        /// <param name="mediator"> An <see cref="IMediator"/> instance.</param>
        public SolutionsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of a Solution.</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetSolutionResult>> GetAsync([Required] string id)
        {
            var result = await mediator.Send(new GetSolutionByIdQuery(id));
            return new GetSolutionResult(result);
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of a Solution.</returns>
        [HttpGet]
        [Route("{id}/Dashboard")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SolutionDashboardResult>> Dashboard([Required] string id)
        {
            var result = await mediator.Send(new GetSolutionByIdQuery(id));
            return Ok(new SolutionDashboardResult(result));
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation of the Dashboard Authority.</returns>
        [HttpGet]
        [Route("{id}/Dashboard/Authority")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SolutionAuthorityDashboardResult>> AuthorityDashboard([Required] string id)
        {
            var result = await mediator.Send(new GetSolutionByIdQuery(id));
            return Ok(new SolutionAuthorityDashboardResult(result));
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of a Solution.</returns>
        [HttpGet]
        [Route("{id}/Preview")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SolutionResult>> Preview([Required] string id)
        {
            var result = await mediator.Send(new GetSolutionByIdQuery(id));
            return Ok(new SolutionResult(result));
        }

        /// <summary>
        /// Get a solution matching the specified ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of a Solution.</returns>
        [HttpGet]
        [Route("{id}/Public")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SolutionResult>> Public([Required] string id)
        {
            var result = await mediator.Send(new GetSolutionByIdQuery(id));
            return result?.PublishedStatus != PublishedStatus.Published
                ? (ActionResult)new NotFoundResult()
                : Ok(new SolutionResult(result));
        }

        /// <summary>
        /// Submits a solution for review.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to update the state of a solution to submitted for review.</returns>
        [HttpPut]
        [Route("{id}/SubmitForReview")]
        [ProducesResponseType(typeof(SubmitSolutionForReviewResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> SubmitForReviewAsync([Required] string id)
        {
            SubmitSolutionForReviewCommandResult result = await mediator.Send(new SubmitSolutionForReviewCommand(id));
            return result.IsSuccess
                ? NoContent()
                : (ActionResult)BadRequest(SubmitSolutionForReviewResult.Create(result.Errors));
        }
    }
}
