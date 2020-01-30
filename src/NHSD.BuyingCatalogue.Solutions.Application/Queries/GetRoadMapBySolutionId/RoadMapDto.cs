using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId
{
    internal sealed class RoadMapDto : IRoadMap
    {
        public string Summary { get; set; }
        public string DocumentName { get; set; }
    }
}
