using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class RoadMapResult : IRoadMapResult
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
