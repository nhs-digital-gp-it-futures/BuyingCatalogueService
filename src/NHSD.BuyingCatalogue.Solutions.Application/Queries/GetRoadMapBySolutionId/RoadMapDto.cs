using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetRoadMapBySolutionId
{
    internal class RoadMapDto : IRoadMap
    {
        public string Summary { get; set; }
    }
}
