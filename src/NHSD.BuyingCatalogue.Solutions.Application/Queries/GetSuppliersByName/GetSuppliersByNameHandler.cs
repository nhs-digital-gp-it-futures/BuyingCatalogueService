using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSuppliersByName
{
    internal sealed class GetSuppliersByNameHandler : IRequestHandler<GetSuppliersByNameQuery, IEnumerable<ISupplier>>
    {
        private readonly IMapper mapper;
        private readonly SupplierReader reader;

        public GetSuppliersByNameHandler(SupplierReader reader, IMapper mapper)
        {
            this.reader = reader;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ISupplier>> Handle(GetSuppliersByNameQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<IEnumerable<ISupplier>>(
                await reader.ByNameAsync(request.Name, request.SolutionPublicationStatus, request.CatalogueItemType, cancellationToken));
        }
    }
}
