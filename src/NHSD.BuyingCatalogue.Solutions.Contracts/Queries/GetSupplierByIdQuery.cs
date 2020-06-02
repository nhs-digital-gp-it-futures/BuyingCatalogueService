using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetSupplierByIdQuery : IRequest<ISupplier>
    {
        public GetSupplierByIdQuery(string id) => Id = id;

        public string Id { get; }
    }
}
