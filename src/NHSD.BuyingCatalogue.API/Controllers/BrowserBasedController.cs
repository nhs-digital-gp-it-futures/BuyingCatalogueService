using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowserBasedController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedController"/> class.
        /// </summary>
        public BrowserBasedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the browser-based options for the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/browser-based")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBrowserBasedAsync([FromRoute][Required]string id)
        { 
            var solution = await _mediator.Send(new GetSolutionByIdQuery(id));
            return solution == null ? (ActionResult) new NotFoundResult() : Ok(new BrowserBasedResult(solution.ClientApplication));
        }
    }
}
