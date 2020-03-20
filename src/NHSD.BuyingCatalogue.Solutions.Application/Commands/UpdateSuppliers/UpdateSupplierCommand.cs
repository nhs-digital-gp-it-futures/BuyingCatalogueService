using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers
{
    public sealed class UpdateSupplierCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IUpdateSupplierData Data { get; }

        public UpdateSupplierCommand(string solutionId, IUpdateSupplierData data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}
