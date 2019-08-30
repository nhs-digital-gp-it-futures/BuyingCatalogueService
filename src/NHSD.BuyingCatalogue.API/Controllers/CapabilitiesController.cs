using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    /// <summary>
    /// Provides a set of endpoints for the information related to the entity <see cref="Capability"/>.
    /// </summary>
	[Route("api/v1/[controller]")]
    [ApiController]
    public class CapabilitiesController : ControllerBase
    {
		/// <summary>
		/// The mediator to pass commands and queries through.
		/// </summary>
		public IMediator Mediator { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="SolutionsSummaryController"/> class.
		/// </summary>
		public CapabilitiesController(IMediator mediator)
		{
			Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Gets a list of capabilitiies.
		/// </summary>
		/// <returns>A result containing a list of capabilities.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(ListCapabilitiesResult), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<ListCapabilitiesResult>> ListAsync()
		{
			return Ok(await Mediator.Send(new ListCapabilitiesQuery()));
		}
	}
}
