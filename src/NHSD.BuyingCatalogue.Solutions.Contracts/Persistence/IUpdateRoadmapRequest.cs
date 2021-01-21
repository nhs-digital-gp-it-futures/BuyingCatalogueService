namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateRoadMapRequest
    {
        string SolutionId { get; }

        string Description { get; }
    }
}
