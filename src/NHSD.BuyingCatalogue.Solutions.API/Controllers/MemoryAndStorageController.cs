using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileMemoryAndStorage;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class MemoryAndStorageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MemoryAndStorageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}/sections/mobile-memory-and-storage")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetMemoryAndStorageAsync([FromRoute] [Required] string id)
        {
            var result = new GetSolutionMemoryAndStorageResult
            {
                StorageRequirementsDescription = CannedData.ContainsKey(id) ? CannedData[id].Description : null,
                MinimumMemoryRequirement = CannedData.ContainsKey(id) ? CannedData[id].MinimumMemoryRequirement : null
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/mobile-memory-and-storage")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateMemoryAndStorageAsync([FromRoute] [Required] string id, [FromBody] [Required] UpdateSolutionMemoryAndStorageRequest viewModel)
        {
            CannedData[id] = viewModel.ThrowIfNull();

            var validationResult = await _mediator
                .Send(new UpdateSolutionMobileMemoryStorageCommand(id, viewModel?.MinimumMemoryRequirement,
                    viewModel?.Description)).ConfigureAwait(false);

            return validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : BadRequest(new UpdateMobileMemoryAndStorageResult(validationResult));
        }

        private static readonly Dictionary<string, UpdateSolutionMemoryAndStorageRequest> CannedData = new Dictionary<string, UpdateSolutionMemoryAndStorageRequest>();
    }
}
