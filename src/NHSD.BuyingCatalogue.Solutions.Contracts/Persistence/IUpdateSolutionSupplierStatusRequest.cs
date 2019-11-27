namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateSolutionSupplierStatusRequest
    {
        string Id { get; }

        int SupplierStatusId { get; }
    }
}
