using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetClientApplicationBySolutionIdQuery"/>.
    /// </summary>
    internal sealed class GetClientApplicationBySolutionIdHandler : IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication>
    {
        private readonly ClientApplicationReader _reader;
        private readonly IMapper _mapper;

        public GetClientApplicationBySolutionIdHandler(ClientApplicationReader reader, IMapper mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public async Task<IClientApplication> Handle(GetClientApplicationBySolutionIdQuery request,
            CancellationToken cancellationToken)
            => _mapper.Map<IClientApplication>(await _reader.BySolutionIdAsync(request.Id, cancellationToken).ConfigureAwait(false));
    }
}
