using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult<List<AdditionalServiceResult>> Get([FromQuery] string[] solutionIds)
        {
            if (solutionIds is null)
                return NotFound();

            return new List<AdditionalServiceResult>
            {
                new AdditionalServiceResult
                {
                    AdditionalServiceId = SetAdditionalServiceId(solutionIds),
                    Name = "Write on Time Additional Service 1",
                    Summary = "Addition to Write on Time",
                    Solution = new AdditionalServiceSolutionResult
                    {
                        SolutionId = "100000-001",
                        Name = "Write on Time"
                    }
                }
            };
        }

        private static string SetAdditionalServiceId(string[] solutionIds)
        {
            var additionalServiceId = string.Empty;
            var incrementalId = 1;

            foreach (var solutionId in solutionIds)
            {
                additionalServiceId += $"{solutionId}A{incrementalId.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0')}";

                incrementalId++;
            }

            return additionalServiceId;
        }
    }
}
