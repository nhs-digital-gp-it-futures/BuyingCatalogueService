using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierById
{
    internal sealed class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, ISupplier>
    {
        private readonly IMapper mapper;
        private readonly SupplierReader reader;

        public GetSupplierByIdHandler(SupplierReader reader, IMapper mapper)
        {
            this.reader = reader;
            this.mapper = mapper;
        }

        public async Task<ISupplier> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<ISupplier>(await reader.ByIdAsync(request.Id, cancellationToken));
        }
    }
}
