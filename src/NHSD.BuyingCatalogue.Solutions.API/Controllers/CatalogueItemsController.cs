using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.CatalogueItems;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById;
using NHSD.BuyingCatalogue.Solutions.Contracts;

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetCatalogueItemResult>>> ListAsync([FromQuery] string supplierId, [FromQuery] CatalogueItemType? catalogueItemType)
        {
            var catalogueItemList = await _mediator.Send(new ListCatalogueItemQuery(supplierId, catalogueItemType));

            return catalogueItemList.Select(catalogueItem => new GetCatalogueItemResult
            {
                CatalogueItemId = catalogueItem.CatalogueItemId,
                Name = catalogueItem.Name
            }).ToList();
        }
    }
}
