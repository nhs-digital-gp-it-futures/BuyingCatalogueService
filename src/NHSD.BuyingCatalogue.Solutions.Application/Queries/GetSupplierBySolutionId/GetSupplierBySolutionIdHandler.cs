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
        private readonly SupplierReader reader;
        private readonly IMapper mapper;

        public GetSupplierBySolutionIdHandler(SupplierReader reader, IMapper mapper)
        {
            this.reader = reader;
            this.mapper = mapper;
        }

        public async Task<ISolutionSupplier> Handle(
            GetSupplierBySolutionIdQuery request,
            CancellationToken cancellationToken)
        {
            return mapper.Map<ISolutionSupplier>(await reader.BySolutionIdAsync(request.SolutionId, cancellationToken));
        }
    }
}
