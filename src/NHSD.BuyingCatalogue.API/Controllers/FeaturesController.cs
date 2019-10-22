using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
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
    public sealed class FeaturesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="NHSD.BuyingCatalogue.API.Controllers.SolutionsController"/> class.
        /// </summary>
        public FeaturesController(IMediator mediator)
        {
            _mediator = mediator;
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

        /// <summary>
        /// Gets the features of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/features")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetFeaturesAsync([FromRoute][Required]string id)
        {
            GetSolutionByIdResult result = await _mediator.Send(new GetSolutionByIdQuery(id));
            return result.Solution == null ? (ActionResult)new NotFoundResult() : Ok(Map(result));
        }

        private FeaturesResult Map(GetSolutionByIdResult result)
        {
            var featuresSection = (FeaturesSection)result.Solution.MarketingData.Sections.FirstOrDefault(s => s.Id == "features");
            return new FeaturesResult
            {
                Listing = featuresSection?.Data?.Listing
            };
        }
    }
}
