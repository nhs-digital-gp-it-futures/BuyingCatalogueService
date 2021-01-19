using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.SolutionLists.API.ViewModels;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;

namespace NHSD.BuyingCatalogue.SolutionLists.API
{
    /// <summary>
    /// Provides an endpoint to manage the solution list.
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class SolutionListController : ControllerBase
    {
        private readonly IMediator mediator;

        public SolutionListController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets a list of solutions that includes information about the supplier and the associated capabilities.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <returns>A task representing an operation to retrieve a list of solutions that includes information
        /// about the supplier and the associated capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListAsync(string supplierId)
        {
            return Ok(new ListSolutionsResult(
                await mediator.Send(new ListSolutionsQuery(new ListSolutionsFilterViewModel { SupplierId = supplierId }))));
        }

        /// <summary>
        /// Gets a list of foundation solutions that includes information about the supplier and the associated capabilities.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of solutions that includes information about the supplier and the associated capabilities.</returns>
        [HttpGet]
        [Route("Foundation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListFoundationAsync()
        {
            return Ok(new ListSolutionsResult(
                await mediator.Send(new ListSolutionsQuery(new ListSolutionsFilterViewModel { IsFoundation = true }))));
        }

        /// <summary>
        /// Find a list of Solutions that match the specified list of Capabilities.
        /// </summary>
        /// <param name="filter">Criteria to apply to the list of Solutions.</param>
        /// <returns>A task representing an operation to retrieve a list of Solutions that match a set of Capabilities.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListByFilterAsync(ListSolutionsFilterViewModel filter)
        {
            return Ok(new ListSolutionsResult(await mediator.Send(new ListSolutionsQuery(filter))));
        }
    }
}
