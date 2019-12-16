using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateMobileConnectionDetails;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class MobileConnectionDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MobileConnectionDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/mobile-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetMobileConnectionDetails([FromRoute] [Required] string id)
        {
            var result = new GetMobileConnectionDetailsResult()
            {
                MinimumConnectionSpeed = CannedData.Keys.Contains(id) ? CannedData[id].Item1 : null,
                ConnectionType = CannedData.Keys.Contains(id) ? CannedData[id].Item2 : null,
                ConnectionRequirementsDescription = CannedData.Keys.Contains(id) ? CannedData[id].Item3 : null
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/mobile-connection-details")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult UpdateMobileOperatingSystems([FromRoute] [Required] string id,
            [FromBody] [Required] UpdateSolutionMobileConnectionDetailsViewModel viewModel)
        {
            CannedData[id] = (viewModel.ThrowIfNull().MinimumConnectionSpeed, viewModel.ThrowIfNull().ConnectionType,
                viewModel.ThrowIfNull().ConnectionRequirementsDescription);

            return NoContent();
        }

        //Canned Data
        private static readonly Dictionary<string, (string, IEnumerable<string>, string)> CannedData = new Dictionary<string, (string, IEnumerable<string>, string)>();
    }
}
