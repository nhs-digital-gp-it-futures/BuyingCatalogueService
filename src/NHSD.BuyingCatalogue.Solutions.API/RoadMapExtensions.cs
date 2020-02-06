using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class RoadMapExtensions
    {
        public static bool IsRoadMapComplete(this ISolution solution) =>
            !string.IsNullOrWhiteSpace(solution.RoadMap?.Summary);
    }
}
