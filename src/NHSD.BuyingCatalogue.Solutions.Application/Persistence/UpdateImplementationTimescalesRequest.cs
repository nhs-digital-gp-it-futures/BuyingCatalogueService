using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateImplementationTimescalesRequest : IUpdateImplementationTimescalesRequest
    {
        public UpdateImplementationTimescalesRequest(string solutionId, string description)
        {
            SolutionId = solutionId;
            Description = description;
        }

        public string SolutionId { get; }

        public string Description { get; }
    }
}
