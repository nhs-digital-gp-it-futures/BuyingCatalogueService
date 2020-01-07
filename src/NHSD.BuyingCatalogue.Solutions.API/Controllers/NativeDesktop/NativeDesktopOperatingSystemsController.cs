using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class NativeDesktopOperatingSystemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeDesktopOperatingSystemsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/native-desktop-operating-systems")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetSupportedOperatingSystems([FromRoute] [Required] string id)
        {
            var result = new GetNativeDesktopOperatingSystemsResult()
            {
                OperatingSystemsDescription = CannedData.ContainsKey(id) ? CannedData[id] : null
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-operating-systems")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdatedSupportedOperatingSystems([FromRoute][Required] string id, [FromBody] [Required] UpdateNativeDesktopOperatingSystemsViewModel viewModel) =>
            (await _mediator
                .Send(new UpdateSolutionNativeDesktopOperatingSystemsCommand(id, viewModel.ThrowIfNull().NativeDesktopOperatingSystemsDescription))
                .ConfigureAwait(false)).ToActionResult();

        private static readonly Dictionary<string, string> CannedData = new Dictionary<string, string>();
    }
}
