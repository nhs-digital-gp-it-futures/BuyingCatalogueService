namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetCatalogueItemById
{
    public sealed class CatalogueItemDto
    {
        public CatalogueItemDto(string catalogueItemId, string name)
        {
            CatalogueItemId = catalogueItemId;
            Name = name;
        }

        public string CatalogueItemId { get; }

        public string Name { get; }
    }
}
