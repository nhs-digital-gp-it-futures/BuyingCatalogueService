using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeMobile
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class NativeMobileAdditionalInformationController : ControllerBase
    {
        private readonly IMediator mediator;

        public NativeMobileAdditionalInformationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/native-mobile-additional-information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAdditionalInformationAsync([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));
            return Ok(new GetNativeMobileAdditionalInformationResult(clientApplication?.NativeMobileAdditionalInformation));
        }

        [HttpPut]
        [Route("{id}/sections/native-mobile-additional-information")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAdditionalInformationAsync(
            [Required] string id,
            UpdateNativeMobileAdditionalInformationViewModel model) =>
            (await mediator.Send(new UpdateSolutionNativeMobileAdditionalInformationCommand(
                id,
                model?.NativeMobileAdditionalInformation))).ToActionResult();
    }
}
