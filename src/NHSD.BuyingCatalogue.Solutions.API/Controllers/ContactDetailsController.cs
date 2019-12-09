using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class ContactDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the contact details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the contact details section.</returns>
        [HttpGet]
        [Route("{id}/sections/contact-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetContactDetailsAsync([FromRoute][Required]string id)
        {
            var contactDetails = await _mediator.Send(new GetContactDetailBySolutionIdQuery(id)).ConfigureAwait(false);
            return contactDetails == null ? (ActionResult)new NotFoundResult() : Ok(new GetContactDetailsResult(contactDetails));
        }

        /// <summary>
        /// Updates the contact details of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="updateSolutionContactDetailsViewModel">The details of the contact details.</param>
        /// <returns>A task representing an operation to update the details of the contact details section.</returns>
        [HttpPut]
        [Route("{id}/sections/contact-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateContactDetailsAsync([FromRoute][Required]string id, [FromBody][Required]UpdateSolutionContactDetailsViewModel updateSolutionContactDetailsViewModel)
        {
            var validationResult =
                await _mediator.Send(new UpdateSolutionContactDetailsCommand(id, updateSolutionContactDetailsViewModel)).ConfigureAwait(false);

            return validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : BadRequest(new UpdateSolutionContactDetailsResult(validationResult));
        }
    }
}
