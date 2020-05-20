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
        public ActionResult Get(string name)
        {
            var result = new List<GetSuppliersResult>
            {
                new GetSuppliersResult() {SupplierId = "id", Name = name}
            };

            return Ok(result);
        }
    }
}
