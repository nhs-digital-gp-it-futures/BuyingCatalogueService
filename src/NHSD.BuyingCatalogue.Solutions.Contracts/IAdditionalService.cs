namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IAdditionalService
    {
        string CatalogueItemId { get; }

        string Summary { get; }

        string CatalogueItemName { get; }

        string SolutionId { get; }

        string SolutionName { get; }
    }
}
