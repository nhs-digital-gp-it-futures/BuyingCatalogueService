using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId
{
    internal sealed class GetSupplierBySolutionIdHandler : IRequestHandler<GetSupplierBySolutionIdQuery, ISupplier>
    {
        private readonly SupplierReader _reader;
        private readonly IMapper _mapper;

        public GetSupplierBySolutionIdHandler(SupplierReader reader, IMapper mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public async Task<ISupplier> Handle(GetSupplierBySolutionIdQuery request,
            CancellationToken cancellationToken) =>
            _mapper.Map<ISupplier>(await _reader.BySolutionIdAsync(request.Id, cancellationToken)
                .ConfigureAwait(false));
    }
}
