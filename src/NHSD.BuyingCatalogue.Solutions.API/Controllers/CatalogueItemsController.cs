using System;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/catalogue-items")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class CatalogueItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogueItemsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{catalogueItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCatalogueItemResult>> GetAsync(string catalogueItemId)
        {
            var catalogueItem = await _mediator.Send(new GetCatalogueItemByIdQuery(catalogueItemId));

            return new GetCatalogueItemResult
            {
                CatalogueItemId = catalogueItem?.CatalogueItemId,
                Name = catalogueItem?.Name
            };
        }
    }
}
