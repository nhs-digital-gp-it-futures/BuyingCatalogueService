using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.QueryModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class SuppliersController : ControllerBase
    {
        private readonly IMediator mediator;

        public SuppliersController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSuppliersModel>>> GetList([FromQuery] SupplierSearchQueryModel query)
        {
            var supplierQuery = new GetSuppliersByNameQuery(
                query?.Name,
                query?.SolutionPublicationStatus,
                query?.CatalogueItemType ?? CatalogueItemType.Solution);

            var suppliers = await mediator.Send(supplierQuery);

            return Ok(suppliers.Select(s => new GetSuppliersModel(s)));
        }

        [HttpGet("{supplierId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetSupplierModel>> Get(string supplierId)
        {
            var supplier = await mediator.Send(new GetSupplierByIdQuery(supplierId));
            if (supplier is null)
                return NotFound();

            return Ok(new GetSupplierModel(supplier));
        }
    }
}
