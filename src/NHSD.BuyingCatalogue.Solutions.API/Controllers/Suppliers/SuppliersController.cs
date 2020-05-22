using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class SuppliersController : Controller
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

        [HttpGet]
        [Route("{supplierId}")]
        public ActionResult<GetSupplierResult> Get(string supplierId)
        {
            var result = new GetSupplierResult
            {
                SupplierId = $"SupplierId {supplierId}",
                Name = "Some name",
                Address = new AddressModel
                {
                    Line1 = "Some address",
                    Line2 = "Some Road",
                    Line3 = "Line 3 address",
                    Line4 = "Another line of address",
                    Line5 = "5th line of address",
                    Town = "A Town",
                    County = "Some county",
                    Postcode = "Some postcode",
                    Country = "A country"
                },
                PrimaryContact = new PrimaryContactModel
                {
                    FirstName = "bob",
                    LastName = "smith",
                    EmailAddress = "bob.smith@email.com",
                    TelephoneNumber = "4342345223  3434324"
                }
            };

            return Ok(result);
        }
    }
}
