using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Capabilities.Contracts;

namespace NHSD.BuyingCatalogue.Capabilities.API.ViewModels
{
    /// <summary>
    /// Provides the result details of the <see cref="ListCapabilitiesResult"/>.
    /// </summary>
    public sealed class ListCapabilitiesResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCapabilitiesResult"/> class.
        /// </summary>
        /// <param name="capabilities">The capabilities.</param>
        public ListCapabilitiesResult(IEnumerable<ICapability> capabilities)
            => Capabilities = capabilities.Select(c => new CapabilityViewModel(c));

        /// <summary>
        /// Gets a list of capabilities.
        /// </summary>
        public IEnumerable<CapabilityViewModel> Capabilities { get; }
    }
}
