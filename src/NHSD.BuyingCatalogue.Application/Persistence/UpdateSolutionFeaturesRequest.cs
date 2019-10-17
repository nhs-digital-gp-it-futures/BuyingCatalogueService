using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class UpdateSolutionFeaturesRequest : IUpdateSolutionFeaturesRequest
    {
        public string Id { get; set; }

        public string Features { get; set; }
    }
}
