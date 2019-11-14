namespace NHSD.BuyingCatalogue.Contracts.Persistence
{
    public interface IUpdateSolutionClientApplicationRequest
    {
        string Id { get; }

        string ClientApplication { get; }
    }
}
