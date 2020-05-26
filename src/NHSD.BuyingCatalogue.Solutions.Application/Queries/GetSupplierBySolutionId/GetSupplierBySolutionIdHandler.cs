using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId
{
    internal sealed class GetSupplierBySolutionIdHandler : IRequestHandler<GetSupplierBySolutionIdQuery, ISolutionSupplier>
    {
        private readonly SupplierReader _reader;
        private readonly IMapper _mapper;

        public GetSupplierBySolutionIdHandler(SupplierReader reader, IMapper mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public async Task<ISolutionSupplier> Handle(GetSupplierBySolutionIdQuery request,
            CancellationToken cancellationToken) =>
            _mapper.Map<ISolutionSupplier>(await _reader.BySolutionIdAsync(request.SolutionId, cancellationToken)
                .ConfigureAwait(false));
    }
}
