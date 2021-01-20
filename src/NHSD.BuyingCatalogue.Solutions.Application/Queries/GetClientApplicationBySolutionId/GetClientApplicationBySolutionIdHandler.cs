using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    /// <summary>
    /// Defines the request handler for the <see cref="GetClientApplicationBySolutionIdQuery"/>.
    /// </summary>
    internal sealed class GetClientApplicationBySolutionIdHandler : IRequestHandler<GetClientApplicationBySolutionIdQuery, IClientApplication>
    {
        private readonly ClientApplicationReader reader;
        private readonly IMapper mapper;

        public GetClientApplicationBySolutionIdHandler(ClientApplicationReader reader, IMapper mapper)
        {
            this.reader = reader;
            this.mapper = mapper;
        }

        public async Task<IClientApplication> Handle(
            GetClientApplicationBySolutionIdQuery request,
            CancellationToken cancellationToken)
        {
            return mapper.Map<IClientApplication>(await reader.BySolutionIdAsync(request.Id, cancellationToken));
        }
    }
}
