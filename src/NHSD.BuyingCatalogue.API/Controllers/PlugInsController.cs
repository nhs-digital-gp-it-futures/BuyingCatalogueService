using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class PlugInsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="PlugInsController"/> class.
        /// </summary>
        public PlugInsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the browsers supported for the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the browsers supported section.</returns>
        [HttpGet]
        [Route("{id}/sections/plug-ins-or-extensions")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPlugInsAsync([FromRoute][Required]string id)
        {
            //Canned data
            //return Ok(new GetPlugInsResult { PlugIns = _plugIns, AdditionalInformation = _additionalInformation });

            var solution = await _mediator.Send(new GetSolutionByIdQuery(id));
            return solution == null ? (ActionResult)new NotFoundResult() : Ok(new GetPlugInsResult(solution.ClientApplication.Plugins));
        }

        /// <summary>
        /// Updates the browsers supported of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="updateSolutionPlugInsViewModel">The details of the supported browsers.</param>
        /// <returns>A task representing an operation to update the details of the browser supported section.</returns>
        [HttpPut]
        [Route("{id}/sections/plug-ins-or-extensions")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdatePlugInsAsync([FromRoute][Required]string id, [FromBody][Required]UpdateSolutionPluginsViewModel updateSolutionPlugInsViewModel)
        {
            var validationResult =
                await _mediator.Send(new UpdateSolutionPluginsCommand(id, updateSolutionPlugInsViewModel));

            //TODO REMOVE when complete - canned data
            _plugIns = updateSolutionPlugInsViewModel.Required;
            _additionalInformation = updateSolutionPlugInsViewModel.AdditionalInformation;

            return validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : BadRequest(new UpdateSolutionPluginsResult(validationResult));
        }

        private static string _plugIns;

        private static string _additionalInformation;
    }
}
