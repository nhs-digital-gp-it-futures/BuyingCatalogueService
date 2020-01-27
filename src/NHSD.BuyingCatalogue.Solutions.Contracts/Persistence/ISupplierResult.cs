namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISupplierResult
    {
        string SolutionId { get; }

        string Name { get; }

        string Summary { get; }

        string Url { get; }
    }
}
