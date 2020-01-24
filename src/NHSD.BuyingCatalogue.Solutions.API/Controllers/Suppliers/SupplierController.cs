using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers.Suppliers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplierController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        [Route("{id}/sections/about-supplier")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get([FromRoute] [Required] string id)
        {
            var aboutSupplier = await _mediator.Send(new GetSupplierBySolutionIdQuery(id)).ConfigureAwait(false);

            return Ok(new GetSupplierResult(aboutSupplier));
        }

        [HttpPut]
        [Route("{id}/sections/about-supplier")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update([FromRoute] [Required] string id, [FromBody] [Required] UpdateSupplierViewModel viewModel) =>
            (await _mediator.Send(new UpdateSupplierCommand(id, viewModel)).ConfigureAwait(false)).ToActionResult();
    }
}
