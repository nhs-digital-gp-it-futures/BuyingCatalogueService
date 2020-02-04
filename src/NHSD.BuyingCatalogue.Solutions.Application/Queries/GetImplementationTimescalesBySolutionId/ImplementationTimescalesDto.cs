using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetImplementationTimescalesBySolutionId
{
    internal sealed class ImplementationTimescalesDto : IImplementationTimescales
    {
        public string Description { get; set; }
    }
}
