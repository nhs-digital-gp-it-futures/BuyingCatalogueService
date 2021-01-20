using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetHostingBySolutionId
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetHostingBySolutionIdQuery"/>.
    /// </summary>
    internal sealed class GetHostingBySolutionIdHandler : IRequestHandler<GetHostingBySolutionIdQuery, IHosting>
    {
        private readonly HostingReader reader;
        private readonly IMapper mapper;

        public GetHostingBySolutionIdHandler(HostingReader reader, IMapper mapper)
        {
            this.reader = reader;
            this.mapper = mapper;
        }

        public async Task<IHosting> Handle(
            GetHostingBySolutionIdQuery request,
            CancellationToken cancellationToken)
        {
            return mapper.Map<IHosting>(await reader.BySolutionIdAsync(request.Id, cancellationToken));
        }
    }
}
