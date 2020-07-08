namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IAdditionalServiceResult
    {
        string CatalogueItemId { get; }

        string CatalogueItemName { get; set; }

        string Summary { get; }

        string SolutionId { get; }

        string SolutionName { get; }
    }
}
