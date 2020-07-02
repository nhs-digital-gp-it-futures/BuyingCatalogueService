using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.AdditionalService;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/additional-services")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class AdditionalServiceController : ControllerBase
    {
        [HttpGet]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Cannot be viewed on swagger if static")]
        public ActionResult<AdditionalServiceResult> Get([FromQuery] string[] solutionIds)
        {
            return new AdditionalServiceResult
            {
                Id = solutionIds.FirstOrDefault(),
                Name = "Write on Time Additional Service 1",
                Summary = "Addition to Write on Time",
                Solution = new AdditionalServiceSolutionResult
                {
                    Id = "100000-001",
                    Name = "Write on Time"
                }
            };
        }
    }
}
