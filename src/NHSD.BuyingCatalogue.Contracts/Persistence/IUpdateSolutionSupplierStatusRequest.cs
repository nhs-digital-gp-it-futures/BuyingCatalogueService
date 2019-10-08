namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface IUpdateSolutionSupplierStatusRequest
    {
        string Id { get; }

        int SupplierStatusId { get; }
    }
}
