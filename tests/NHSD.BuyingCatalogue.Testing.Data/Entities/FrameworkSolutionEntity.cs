namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class FrameworkSolutionEntity : EntityBase
    {
        public string FrameworkId { get; set; }

        public string SolutionId { get; set; }

        public bool IsFoundation { get; set; }

        protected override string InsertSql => @"
            INSERT INTO dbo.FrameworkSolutions
            (
                FrameworkId,
                SolutionId,
                IsFoundation,
                LastUpdated,
                LastUpdatedBy
            )
            VALUES
            (
                @FrameworkId,
                @SolutionId,
                @IsFoundation,
                @LastUpdated,
                @LastUpdatedBy
            );";
    }
}
