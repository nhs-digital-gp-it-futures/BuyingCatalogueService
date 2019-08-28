using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
    public sealed class ListCapabilitiesQueryHandler : IRequestHandler<ListCapabilitiesQuery, ListCapabilitiesQueryResult>
    {
        /// <summary>
        /// Data access layer for the <see cref="Capability"/> entity.
        /// </summary>
        private ICapabilityRepository CapabilityRepository { get; }

        /// <summary>
        /// The mapper to convert one object into another type.
        /// </summary>
        private IMapper Mapper { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListCapabilitiesQueryHandler"/> class.
        /// </summary>
        public ListCapabilitiesQueryHandler(ICapabilityRepository capabilityRepository, IMapper mapper)
        {
            CapabilityRepository = capabilityRepository ?? throw new System.ArgumentNullException(nameof(capabilityRepository));
            Mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="request">The query parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The result of the query.</returns>
        public async Task<ListCapabilitiesQueryResult> Handle(ListCapabilitiesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Capability> capabilities = await CapabilityRepository.ListAsync(cancellationToken).ConfigureAwait(false);

            return new ListCapabilitiesQueryResult
            {
                Capabilities = Mapper.Map<IEnumerable<CapabilityViewModel>>(capabilities)
            };
        }
    }
}
