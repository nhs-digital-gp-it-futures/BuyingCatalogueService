namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierResult
    {
        string SolutionId { get; }

        string Description { get; }

        string Link { get; }
    }
}
