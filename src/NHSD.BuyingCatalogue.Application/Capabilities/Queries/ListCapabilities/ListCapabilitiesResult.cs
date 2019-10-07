using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
    /// <summary>
    /// Provides the result details of the <see cref="ListCapabilitiesQuery"/>.
    /// </summary>
	public sealed class ListCapabilitiesResult
	{
		/// <summary>
		/// A list of capabilities.
		/// </summary>
		public IEnumerable<CapabilityViewModel> Capabilities { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="ListCapabilitiesResult"/> class.
		/// </summary>
		public ListCapabilitiesResult(IEnumerable<CapabilityViewModel> capabilities)
        {
            Capabilities = capabilities;
        }
	}
}
