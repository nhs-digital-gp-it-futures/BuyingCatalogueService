using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Capabilities.Application.Persistence;
using NHSD.BuyingCatalogue.Capabilities.Contracts;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities
{
    /// <summary>
    /// Defines the request handler for the <see cref="ListCapabilitiesQuery"/>.
    /// </summary>
    internal sealed class ListCapabilitiesHandler : IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>>
    {
        private readonly CapabilityReader capabilityReader;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCapabilitiesHandler"/> class.
        /// </summary>
        /// <param name="capabilityReader">The <see cref="CapabilityReader"/> instance.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance.</param>
        public ListCapabilitiesHandler(CapabilityReader capabilityReader, IMapper mapper)
        {
            this.capabilityReader = capabilityReader;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The result of the query.</returns>
        public async Task<IEnumerable<ICapability>> Handle(ListCapabilitiesQuery request, CancellationToken cancellationToken) =>
            mapper.Map<IEnumerable<CapabilityDto>>(await capabilityReader.ListAsync(cancellationToken));
    }
}
