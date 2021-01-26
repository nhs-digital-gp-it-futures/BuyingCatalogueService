namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class AdditionalServiceEntity : EntityBase
    {
        public string CatalogueItemId { get; set; }

        public string Summary { get; set; }

        public string FullDescription { get; set; }

        public string SolutionId { get; set; }

        protected override string InsertSql => @"
            INSERT INTO dbo.AdditionalService
            (
                CatalogueItemId,
                Summary,
                FullDescription,
                SolutionId
            )
            VALUES
            (
                @CatalogueItemId,
                @Summary,
                @FullDescription,
                @SolutionId
            );";
    }
}
