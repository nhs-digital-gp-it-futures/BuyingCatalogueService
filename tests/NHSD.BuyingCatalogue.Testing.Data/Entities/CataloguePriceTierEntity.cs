namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class CataloguePriceTierEntity : EntityBase
    {
        public int CataloguePriceId { get; set; }
        public int BandStart { get; set; }
        public int? BandEnd { get; set; }
        public decimal Price { get; set; }

        protected override string InsertSql => @"
        INSERT INTO dbo.CataloguePriceTier
        (
            CataloguePriceId,
            BandStart,
            BandEnd,
            Price
        )
        VALUES
        (
            @CataloguePriceId,
            @BandStart,
            @BandEnd,
            @Price
        );";
    }
}
