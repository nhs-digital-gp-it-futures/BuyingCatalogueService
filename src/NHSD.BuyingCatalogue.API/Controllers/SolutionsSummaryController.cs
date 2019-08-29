using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides the endpoint for the summary of <see cref="Solution"/> information.
    /// </summary>
    [Route("api/v1/[controller]")]
	[ApiController]
	public sealed class SolutionsSummaryController : ControllerBase
	{
		/// <summary>
		/// The mediator to pass commands and queries through.
		/// </summary>
		public IMediator Mediator { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="SolutionsSummaryController"/> class.
		/// </summary>
		public SolutionsSummaryController(IMediator mediator)
		{
			Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Gets a list of solutions that includes information about the organisation and the associated capabilities.
		/// </summary>
		/// <returns>A result containing a list of solutions that includes information about the organisation and the associated capabilities.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(GetAllSolutionSummariesQueryResult), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<GetAllSolutionSummariesQueryResult>> GetAllAsync()
		{
			return Ok(await Mediator.Send(new GetAllSolutionSummariesQuery()));
		}

        [HttpPost]
        [ProducesResponseType(typeof(GetAllSolutionSummariesQueryResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GetAllSolutionSummariesQueryResult>> FindAsync([FromBody]GetAllSolutionSummariesQueryFilter filter)
        {
            return Ok(await Mediator.Send(new GetAllSolutionSummariesQuery(filter)));
        }
	}
}
