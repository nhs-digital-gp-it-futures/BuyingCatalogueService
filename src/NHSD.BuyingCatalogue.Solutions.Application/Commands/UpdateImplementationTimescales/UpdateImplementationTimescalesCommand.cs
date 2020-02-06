using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales
{
    public sealed class UpdateImplementationTimescalesCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }
        public string Description { get; }

        public UpdateImplementationTimescalesCommand(string solutionId, string description)
        {
            SolutionId = solutionId.ThrowIfNull();
            Description = description;
        }
    }
}
