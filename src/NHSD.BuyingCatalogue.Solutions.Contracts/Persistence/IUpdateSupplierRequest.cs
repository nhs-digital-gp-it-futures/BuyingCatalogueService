namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateSupplierRequest
    {
        string SolutionId { get; }

        string Description { get; }

        string Link { get; }
    }
}
