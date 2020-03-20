namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IDocumentResult
    {
        string RoadMapDocumentName { get; }

        string IntegrationDocumentName { get; }

        string SolutionDocumentName { get; }
    }
}
