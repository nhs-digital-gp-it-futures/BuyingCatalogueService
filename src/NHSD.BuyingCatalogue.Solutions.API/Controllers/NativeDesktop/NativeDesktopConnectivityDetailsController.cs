using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop
{
    public sealed class NativeDesktopConnectivityDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NativeDesktopConnectivityDetailsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/native-desktop-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetConnectivity([FromRoute] [Required] string id)
        {
            var result = new GetNativeDesktopConnectivityDetails()
            {
                NativeDesktopMinimumConnectionSpeed = CannedData.ContainsKey(id) ? CannedData[id] : null
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/native-desktop-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult UpdatedConnectivity([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateNativeDesktopConnectivityDetailsViewModel viewModel)
        {
            CannedData[id] = viewModel.ThrowIfNull().NativeDesktopMinimumConnectionSpeed;
            return NoContent();
        }

        private static readonly Dictionary<string, string> CannedData = new Dictionary<string, string>();
    }
}
