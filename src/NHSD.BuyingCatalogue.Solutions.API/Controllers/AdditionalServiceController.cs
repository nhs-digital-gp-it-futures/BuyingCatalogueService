using System.Collections.Generic;
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
        public ActionResult<IEnumerable<AdditionalServiceResult>> Get([FromQuery] string[] solutionIds)
        {
            if (solutionIds is null)
                return NotFound();

            var additionalServiceResults = new List<AdditionalServiceResult>();

            foreach (var solutionId in solutionIds)
            {
                additionalServiceResults.Add(new AdditionalServiceResult
                {
                    AdditionalServiceId = SetAdditionalServiceId(solutionId),
                    Name = "Write on Time Additional Service 1",
                    Summary = "Addition to Write on Time",
                    Solution = new AdditionalServiceSolutionResult
                    {
                        SolutionId = "100000-001",
                        Name = "Write on Time"
                    }
                });
            }

            return additionalServiceResults;
        }

        private static string SetAdditionalServiceId(string solutionId)
        {
            var additionalServiceId = string.Empty;
            additionalServiceId += $"{solutionId}A001";

            return additionalServiceId;
        }
    }
}
