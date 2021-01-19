using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication
{
    /// <summary>
    /// Provides an endpoint to manage the client application types for a given solution.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class ClientApplicationTypeController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientApplicationTypeController"/> class.
        /// </summary>
        /// <param name="mediator">An <see cref="IMediator"/> instance.</param>
        public ClientApplicationTypeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the client application types section for a given solution.</returns>
        [HttpGet]
        [Route("{id}/sections/client-application-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetClientApplicationTypesAsync([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));
            return Ok(new GetClientApplicationTypesResult(clientApplication));
        }

        /// <summary>
        /// Updates the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of a client application type that includes any updated information.</param>
        /// <returns>A task representing an operation to update the details of the client application types for a solution.</returns>
        [HttpPut]
        [Route("{id}/sections/client-application-types")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateClientApplicationTypesAsync(
            [Required] string id,
            UpdateSolutionClientApplicationTypesViewModel model) =>
            (await mediator.Send(
                new UpdateSolutionClientApplicationTypesCommand(id, model))).ToActionResult();
    }
}
