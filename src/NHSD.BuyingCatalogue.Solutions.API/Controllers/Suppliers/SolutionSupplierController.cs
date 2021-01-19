using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class SolutionSupplierController : ControllerBase
    {
        private readonly IMediator mediator;

        public SolutionSupplierController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/about-supplier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([Required] string id)
        {
            var aboutSupplier = await mediator.Send(new GetSupplierBySolutionIdQuery(id));

            return Ok(new GetSolutionSupplierResult(aboutSupplier));
        }

        [HttpPut]
        [Route("{id}/sections/about-supplier")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([Required] string id, UpdateSolutionSupplierViewModel model) =>
            (await mediator.Send(new UpdateSolutionSupplierCommand(id, model))).ToActionResult();
    }
}
