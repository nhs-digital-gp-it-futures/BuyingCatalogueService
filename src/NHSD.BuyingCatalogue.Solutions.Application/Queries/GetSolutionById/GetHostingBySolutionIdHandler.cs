using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetHostingBySolutionIdQuery"/>.
    /// </summary>
    internal sealed class GetHostingBySolutionIdHandler : IRequestHandler<GetHostingBySolutionIdQuery, IHosting>
    {
        private readonly HostingReader _reader;
        private readonly IMapper _mapper;

        public GetHostingBySolutionIdHandler(HostingReader reader, IMapper mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public async Task<IHosting> Handle(GetHostingBySolutionIdQuery request,
            CancellationToken cancellationToken) =>
            _mapper.Map<IHosting>(await _reader.BySolutionIdAsync(request.Id, cancellationToken).ConfigureAwait(false));
    }
}
