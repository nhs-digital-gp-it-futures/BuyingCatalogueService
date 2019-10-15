using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides the endpoint for the summary of <see cref="Solution"/> information.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class SectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="NHSD.BuyingCatalogue.API.Controllers.SolutionsController"/> class.
        /// </summary>
        public SectionsController(IMediator mediator)
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
        /// Updates the features of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="updateSolutionFeaturesViewModel">The details of a solution that includes any updated inforamtion.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpPut]
        [Route("{id}/sections/features")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateFeaturesAsync([FromRoute][Required]string id, [FromBody][Required]UpdateSolutionFeaturesViewModel updateSolutionFeaturesViewModel)
        {
            await _mediator.Send(new UpdateSolutionFeaturesCommand(id, updateSolutionFeaturesViewModel));

            return NoContent();
        }
    }
}
