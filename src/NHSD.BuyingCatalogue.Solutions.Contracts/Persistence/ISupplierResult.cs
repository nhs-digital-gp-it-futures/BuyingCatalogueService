namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierResult
    {
        string SolutionId { get; }

        string Name { get; }

        string Description { get; }

        string Link { get; }
    }
}
