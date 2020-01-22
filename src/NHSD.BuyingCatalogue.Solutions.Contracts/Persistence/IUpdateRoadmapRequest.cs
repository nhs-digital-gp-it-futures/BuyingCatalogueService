namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateRoadmapRequest
    {
        string SolutionId { get; }
        string Description { get; }
    }
}
