using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.AdditionalService;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/additional-services")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class AdditionalServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdditionalServiceController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<ActionResult<AdditionalServiceListResult>> GetAsync([FromQuery] IEnumerable<string> solutionIds)
        {
            if (solutionIds is null)
            {
                return NotFound();
            }

            solutionIds = solutionIds.ToList();
            if (!solutionIds.Any())
            {
                return NotFound();
            }

            var additionalServices = (await _mediator.Send(new GetAdditionalServiceBySolutionIdsQuery(solutionIds))).ToList();

            return new AdditionalServiceListResult
            {
                AdditionalServices = additionalServices.Select(
                    additionalService => new AdditionalServiceModel
                    {
                        CatalogueItemId = additionalService.CatalogueItemId,
                        Name = additionalService.CatalogueItemName,
                        Summary = additionalService.Summary,
                        Solution = new AdditionalServiceSolutionModel
                        {
                            SolutionId = additionalService.SolutionId,
                            Name = additionalService.SolutionName
                        }
                    })
            };
        }
    }
}
