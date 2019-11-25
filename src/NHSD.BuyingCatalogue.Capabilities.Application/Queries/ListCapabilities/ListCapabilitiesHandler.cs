using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Capabilities.Application.Persistence;
using NHSD.BuyingCatalogue.Contracts.Capability;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities
{
    /// <summary>
	/// Defines the request handler for the <see cref="ListCapabilitiesQuery"/>.
	/// </summary>
    internal sealed class ListCapabilitiesHandler : IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>>
    {
        private readonly CapabilityReader _capabilityReader;

        /// <summary>
        /// The mapper to convert one object into another type.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="ListCapabilitiesHandler"/> class.
        /// </summary>
        public ListCapabilitiesHandler(CapabilityReader capabilityReader, IMapper mapper)
        {
            _capabilityReader = capabilityReader;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The result of the query.</returns>
        public async Task<IEnumerable<ICapability>> Handle(ListCapabilitiesQuery request, CancellationToken cancellationToken) =>
            _mapper.Map<IEnumerable<CapabilityDto>>(await _capabilityReader.ListAsync(cancellationToken).ConfigureAwait(false));
    }
}
