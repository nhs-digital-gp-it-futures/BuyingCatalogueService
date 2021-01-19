using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    /// <summary>
    /// Provides the endpoint to manage the solution description section of the solution details data.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class SolutionDescriptionController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionDescriptionController"/> class.
        /// </summary>
        /// <param name="mediator"> An <see cref="IMediator"/> instance.</param>
        public SolutionDescriptionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the solution description section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the solution description section of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/solution-description")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSolutionDescriptionAsync([Required] string id)
        {
            var solution = await mediator.Send(new GetSolutionByIdQuery(id));
            return Ok(new SolutionDescriptionResult(solution.Summary, solution.Description, solution.AboutUrl));
        }

        /// <summary>
        /// Updates the solution description section of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of a solution description section that includes any updated information.</param>
        /// <returns>A task representing an operation to update the details of the solution description section of a solution.</returns>
        [HttpPut]
        [Route("{id}/sections/solution-description")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAsync([Required] string id, UpdateSolutionSummaryViewModel model) =>
            (await mediator.Send(new UpdateSolutionSummaryCommand(id, model))).ToActionResult();
    }
}
