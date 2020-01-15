using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopMemoryAndStorageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeDesktopMemoryAndStorageController(IMediator mediator)
        {
            mediator.ThrowIfNull();
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/native-desktop-memory-and-storage")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get([FromRoute] [Required] string id)
        {
            var clientApplication =
                await _mediator.Send(new GetClientApplicationBySolutionIdQuery(id)).ConfigureAwait(false);

            return Ok(new GetNativeDesktopMemoryAndStorageResult(clientApplication?.NativeDesktopMemoryAndStorage));
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-memory-and-storage")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeDesktopMemoryAndStorageViewModel viewModel)
        {
            return (await _mediator.Send(
                    new UpdateNativeDesktopMemoryAndStorageCommand(id, viewModel))
                .ConfigureAwait(false)).ToActionResult();
        }
    }
}
