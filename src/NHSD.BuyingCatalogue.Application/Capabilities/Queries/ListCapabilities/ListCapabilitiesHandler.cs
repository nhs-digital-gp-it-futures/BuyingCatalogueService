using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
    /// <summary>
	/// Defines the request handler for the <see cref="ListCapabilitiesQuery"/>.
	/// </summary>
    internal sealed class ListCapabilitiesHandler : IRequestHandler<ListCapabilitiesQuery, ListCapabilitiesResult>
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
        public async Task<ListCapabilitiesResult> Handle(ListCapabilitiesQuery request, CancellationToken cancellationToken)
        {
            var capabilities = await _capabilityReader.ListAsync(cancellationToken).ConfigureAwait(false);

            return new ListCapabilitiesResult(_mapper.Map<IEnumerable<CapabilityViewModel>>(capabilities));
        }
    }
}
