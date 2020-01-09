using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop
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
