using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Contracts.Capability;

namespace NHSD.BuyingCatalogue.Capabilities.API.ViewModels
{
    /// <summary>
    /// Provides the result details of the <see cref="ListCapabilitiesResult"/>.
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
		public ListCapabilitiesResult(IEnumerable<ICapability> capabilities)
            => Capabilities = capabilities.Select(c => new CapabilityViewModel(c));
    }
}
