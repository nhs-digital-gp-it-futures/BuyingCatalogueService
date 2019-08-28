using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
	public sealed class ListCapabilitiesQueryResult
	{
		/// <summary>
		/// A list of capabilities.
		/// </summary>
		public IEnumerable<CapabilityViewModel> Capabilities { get; set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="ListCapabilitiesQueryResult"/> class.
		/// </summary>
		public ListCapabilitiesQueryResult()
		{
		}
	}
}
