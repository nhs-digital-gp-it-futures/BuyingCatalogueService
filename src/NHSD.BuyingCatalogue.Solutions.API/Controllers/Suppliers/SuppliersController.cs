using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers
{
    [Route("api/v1/suppliers")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class SuppliersController : Controller
    {
        [HttpGet]
        public ActionResult GetList(string name)
        {
            var result = new List<GetSuppliersNameResult>
            {
                new GetSuppliersNameResult() {SupplierId = "id", Name = name}
            };

            return Ok(result);
        }
        
        [HttpGet]
        [Route("{supplierId}")]
        public ActionResult Get(string supplierId)
        {
            var result = new GetSuppliersResult
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
