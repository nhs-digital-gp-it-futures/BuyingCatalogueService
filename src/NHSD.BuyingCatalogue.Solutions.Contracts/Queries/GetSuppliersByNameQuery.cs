using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetSuppliersByNameQuery : IRequest<IEnumerable<ISupplier>>
    {
        public GetSuppliersByNameQuery(string name, string solutionPublicationStatus)
        {
            Name = name;
            SolutionPublicationStatus = solutionPublicationStatus;
        }

        public string Name { get; }

        public string SolutionPublicationStatus { get; }
    }
}
