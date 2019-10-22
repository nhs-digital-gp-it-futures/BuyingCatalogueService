using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetClientApplicationTypes;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides the endpoint for the solution-description of <see cref="Solution"/> information.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class SolutionDescriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="NHSD.BuyingCatalogue.API.Controllers.SolutionsController"/> class.
        /// </summary>
        public SolutionDescriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Updates the summary of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="updateSolutionSummaryViewModel">The details of a solution that includes any updated inforamtion.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpPut]
        [Route("{id}/sections/solution-description")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateAsync([FromRoute][Required]string id, [FromBody][Required]UpdateSolutionSummaryViewModel updateSolutionSummaryViewModel)
        {
            await _mediator.Send(new UpdateSolutionSummaryCommand(id, updateSolutionSummaryViewModel));

            return NoContent();
        }

        /// <summary>
        /// Gets the solution description of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/solution-description")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetSolutionDescriptionAsync([FromRoute][Required]string id)
        {
            GetSolutionByIdResult result = await _mediator.Send(new GetSolutionByIdQuery(id));
            return result.Solution == null ? (ActionResult)new NotFoundResult() : Ok(Map(result));
        }

        private SolutionDescriptionResult Map(GetSolutionByIdResult result)
        {
            var solutionDescriptionSection = (SolutionDescriptionSection)result.Solution.MarketingData.Sections.FirstOrDefault(s => s.Id == "solution-description");
            return new SolutionDescriptionResult
            {
                SolutionDescription = new SolutionDescriptionViewModel
                {
                    Description = solutionDescriptionSection?.Data?.Description,
                    Link = solutionDescriptionSection?.Data?.Link,
                    Summary = solutionDescriptionSection?.Data?.Summary,
                }
            };
        }
    }
}
