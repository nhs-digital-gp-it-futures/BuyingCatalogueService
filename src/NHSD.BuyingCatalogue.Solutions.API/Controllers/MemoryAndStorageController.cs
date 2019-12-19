using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class MemoryAndStorageController : ControllerBase
    {
        [HttpGet]
        [Route("{id}/sections/mobile-memory-and-storage")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetMemoryAndStorageAsync([FromRoute] [Required] string id)
        {
            var result = new GetSolutionMemoryAndStorageResult
            {
                StorageRequirementsDescription = CannedData.ContainsKey(id) ? CannedData[id].StorageRequirementsDescription : null,
                MinimumMemoryRequirement = CannedData.ContainsKey(id) ? CannedData[id].MinimumMemoryRequirement : null
            };

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/sections/mobile-memory-and-storage")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult UpdateMemoryAndStorageAsync([FromRoute] [Required] string id, [FromBody] [Required] UpdateSolutionMemoryAndStorageRequest viewModel)
        {
            CannedData[id] = viewModel.ThrowIfNull();
            return NoContent();
        }

        private static readonly Dictionary<string, UpdateSolutionMemoryAndStorageRequest> CannedData = new Dictionary<string, UpdateSolutionMemoryAndStorageRequest>();
    }
}
