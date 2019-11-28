using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class ContactDetailsController : ControllerBase
    {
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
            //Canned data
            return await Task.Run(() => Ok(new GetContactDetailsResult
            {
                Contact1 = Map(_updateSolutionContactDetailsViewModel?.Contact1),
                Contact2 = Map(_updateSolutionContactDetailsViewModel?.Contact2)
            }));
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
            _updateSolutionContactDetailsViewModel = updateSolutionContactDetailsViewModel;


            //var validationResult =
            //    await _mediator.Send(new UpdateSolutionPluginsCommand(id, updateSolutionPlugInsViewModel));

            //return validationResult.IsValid
            //    ? (ActionResult)new NoContentResult()
            //    : BadRequest(new UpdateSolutionPluginsResult(validationResult));

            return await Task.Run(() => NoContent());
        }

        private static UpdateSolutionContactDetailsViewModel _updateSolutionContactDetailsViewModel;

        private GetContactDetailsResultSection Map(UpdateSolutionContactViewModel contact)
        {
            return contact == null
                ? null
                : new GetContactDetailsResultSection
                {
                    DepartmentName = contact.DepartmentName,
                    EmailAddress = contact.EmailAddress,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    PhoneNumber = contact.PhoneNumber
                };
        }
    }
}
