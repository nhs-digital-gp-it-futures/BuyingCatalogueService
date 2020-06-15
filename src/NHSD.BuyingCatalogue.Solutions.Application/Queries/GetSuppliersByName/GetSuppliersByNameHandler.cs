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
        private readonly IMapper _mapper;
        private readonly SupplierReader _reader;

        public GetSuppliersByNameHandler(SupplierReader reader, IMapper mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ISupplier>> Handle(GetSuppliersByNameQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ISupplier>>(
                await _reader.ByNameAsync(request.Name, request.SolutionPublicationStatus, cancellationToken));
        }
    }
}
