using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeMobile
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class MemoryAndStorageController : ControllerBase
    {
        private readonly IMediator mediator;

        public MemoryAndStorageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/native-mobile-memory-and-storage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMemoryAndStorageAsync([Required] string id)
        {
            var clientApplication = await mediator.Send(new GetClientApplicationBySolutionIdQuery(id));

            return Ok(new GetSolutionMemoryAndStorageResult(clientApplication?.MobileMemoryAndStorage));
        }

        [HttpPut]
        [Route("{id}/sections/native-mobile-memory-and-storage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateMemoryAndStorageAsync(
            [Required] string id,
            UpdateSolutionMemoryAndStorageRequest model) =>
            (await mediator.Send(new UpdateSolutionMobileMemoryStorageCommand(
                id,
                model?.MinimumMemoryRequirement,
                model?.Description))).ToActionResult();
    }
}
