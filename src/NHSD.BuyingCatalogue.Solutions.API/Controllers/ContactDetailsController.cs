using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class ContactDetailsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ContactDetailsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the contact details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the contact details section.</returns>
        [HttpGet]
        [Route("{id}/sections/contact-details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetContactDetailsAsync([Required] string id)
        {
            var contactDetails = await mediator.Send(new GetContactDetailBySolutionIdQuery(id));
            return Ok(new GetContactDetailsResult(contactDetails));
        }

        /// <summary>
        /// Updates the contact details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="model">The details of the contact details.</param>
        /// <returns>A task representing an operation to update the details of the contact details section.</returns>
        [HttpPut]
        [Route("{id}/sections/contact-details")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateContactDetailsAsync(
            [Required] string id,
            UpdateSolutionContactDetailsViewModel model) =>
            (await mediator.Send(new UpdateSolutionContactDetailsCommand(id, model))).ToActionResult();
    }
}
