using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [AllowAnonymous]
    public sealed class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSuppliersModel>>> GetList(string name)
        {
            var suppliers = await _mediator.Send(new GetSuppliersByNameQuery(name));

            return Ok(suppliers.Select(s => new GetSuppliersModel(s)));
        }

        [HttpGet("{supplierId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetSupplierModel>> Get(string supplierId)
        {
            var supplier = await _mediator.Send(new GetSupplierByIdQuery(supplierId));
            if (supplier is null)
                return NotFound();

            return Ok(new GetSupplierModel(supplier));
        }
    }
}
