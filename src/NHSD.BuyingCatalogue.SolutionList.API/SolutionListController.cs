using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionList.API.ViewModels;

namespace NHSD.BuyingCatalogue.SolutionList.API
{
    /// <summary>
    /// Provides an endpoint to manage the solution list
    /// </summary>
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public sealed class SolutionListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolutionListController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets a list of solutions that includes information about the organisation and the associated capabilities.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of solutions that includes information about the organisation and the associated capabilities.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ListSolutionsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListAsync()
        {
            return Ok(new ListSolutionsResult(await _mediator.Send(new ListSolutionsQuery())));
        }

        /// <summary>
        /// Gets a list of foundation solutions that includes information about the organisation and the associated capabilities.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of solutions that includes information about the organisation and the associated capabilities.</returns>
        [HttpGet]
        [Route("Foundation")]
        [ProducesResponseType(typeof(ListSolutionsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListFoundationAsync()
        {
            return Ok(new ListSolutionsResult(await _mediator.Send(new ListSolutionsQuery(ListSolutionsFilter.Foundation))));
        }

        /// <summary>
        /// Find a list of Solutions that match the specified list of Capabilities.
        /// </summary>
        /// <param name="filter">The filter criteria to apply to the list of Solutions.</param>
        /// <returns>A task representing an operation to retrieve a list of Solutions that match a set of Capabilities.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ListSolutionsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ListSolutionsResult>> ListByFilterAsync([FromBody][Required]ListSolutionsFilter filter)
        {
            return Ok(new ListSolutionsResult(await _mediator.Send(new ListSolutionsQuery(filter))));
        }
    }
}
