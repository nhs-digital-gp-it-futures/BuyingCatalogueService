using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class NativeDesktopController : ControllerBase
    {
        private readonly IMediator mediator;

        public NativeDesktopController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/dashboards/native-desktop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetNativeDesktopAsync([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));

            return Ok(new NativeDesktopResult(clientApplication));
        }
    }
}
