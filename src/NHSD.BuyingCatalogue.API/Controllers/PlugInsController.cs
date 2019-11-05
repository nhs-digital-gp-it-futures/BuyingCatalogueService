using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.ViewModels;

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
        [Route("{id}/sections/browser-based-plug-ins")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPlugInsAsync([FromRoute][Required]string id)
        {
            //Canned data
            return Ok(new GetPlugInsResult { PlugIns = _plugIns, AdditionalInformation = _additionalInformation });


            //var solution = await _mediator.Send(new GetSolutionByIdQuery(id));
            //return solution == null ? (ActionResult)new NotFoundResult() : Ok(new GetPlugInsResult(solution.ClientApplication));
        }

        /// <summary>
        /// Updates the browsers supported of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="updateSolutionPlugInsViewModel">The details of the supported browsers.</param>
        /// <returns>A task representing an operation to update the details of the browser supported section.</returns>
        [HttpPut]
        [Route("{id}/sections/browser-based-plug-ins")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdatePlugInsAsync([FromRoute][Required]string id, [FromBody][Required]UpdateSolutionPlugInsViewModel updateSolutionPlugInsViewModel)
        {

            _plugIns = updateSolutionPlugInsViewModel.PlugIns;
            _additionalInformation = updateSolutionPlugInsViewModel.AdditionalInformation;

            //await _mediator.Send(new UpdateSolutionPlugInsCommand(id, updateSolutionPlugInsViewModel));

            return NoContent();
        }

        private static string _plugIns;

        private static string _additionalInformation;

    }

    public class UpdateSolutionPlugInsViewModel {

        [JsonProperty("plug-ins")]
        public string PlugIns { get; set; }

        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; set; }
    }
}
