using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
        [Route("api/v1/solutions")]
        [ApiController]
        [Produces("application/json")]
        [AllowAnonymous]
        public class MobileOperatingSystemsController : ControllerBase
        {
            private readonly IMediator _mediator;

            public MobileOperatingSystemsController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpGet]
            [Route("{id}/sections/mobile-operating-systems")]
            [ProducesResponseType((int)HttpStatusCode.BadRequest)]
            [ProducesResponseType((int)HttpStatusCode.NoContent)]
            [ProducesResponseType((int)HttpStatusCode.NotFound)]
            public ActionResult GetMobileOperatingSystems([FromRoute] [Required] string id)
            {
                var result = new GetMobileOperatingSystems()
                {
                    OperatingSystems = CannedData.Keys.Contains(id) ? CannedData[id].Item1 : null,
                    OperatingSystemsDescription = CannedData.Keys.Contains(id) ? CannedData[id].Item2 : null
                };

                return Ok(result);
            }

            [HttpPut]
            [Route("{id}/sections/mobile-operating-systems")]
            [ProducesResponseType((int)HttpStatusCode.BadRequest)]
            [ProducesResponseType((int)HttpStatusCode.NoContent)]
            [ProducesResponseType((int)HttpStatusCode.NotFound)]
            public ActionResult UpdateMobileOperatingSystems([FromRoute] [Required] string id,
                [FromBody] [Required] UpdateSolutionMobileOperatingSystemsViewModel viewModel)
            {
                CannedData[id] = (viewModel.ThrowIfNull().OperatingSystems,
                    viewModel.ThrowIfNull().OperatingSystemsDescription);
                return NoContent();
        }

            //Canned Data
            private static readonly Dictionary<string, (IEnumerable<string>, string)> CannedData = new Dictionary<string, (IEnumerable<string>, string)>();
        }
}
