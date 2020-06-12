using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionEntity : EntityBase
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public string Summary { get; set; }

        public string FullDescription { get; set; }

        public string Features { get; set; }

        public string ClientApplication { get; set; }

        public string Hosting { get; set; }

        public string ImplementationDetail { get; set; }

        public string RoadMap { get; set; }

        public string IntegrationsUrl { get; set; }

        public string AboutUrl { get; set; }

        public string ServiceLevelAgreement { get; set; }

        public string WorkOfPlan { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO dbo.Solution
        (
            [Id],
	        Version,
            Summary,
            FullDescription,
            Features,
            ClientApplication,
            Hosting,
            ImplementationDetail,
            RoadMap,
            IntegrationsUrl,
            AboutUrl,
	        ServiceLevelAgreement,
	        WorkOfPlan,
	        LastUpdated,
	        LastUpdatedBy
        )
        VALUES
        (
            @Id,           
            @Version,   
            @Summary,
            @FullDescription,
            @Features,
            @ClientApplication,
            @Hosting,
            @ImplementationDetail,
            @RoadMap,
            @IntegrationsUrl,
            @AboutUrl,
            @ServiceLevelAgreement,
            @WorkOfPlan,
            @LastUpdated,
            @LastUpdatedBy
        );";

        public static async Task<IEnumerable<SolutionEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<SolutionEntity>($@"SELECT [Id]
                                Version,
                                Summary,
                                FullDescription,
                                Features,
                                ClientApplication,
                                Hosting,
                                ImplementationDetail,
                                RoadMap,
                                IntegrationsUrl,
                                AboutUrl,
	                            ServiceLevelAgreement,
	                            WorkOfPlan,
	                            LastUpdated,
	                            LastUpdatedBy
                                FROM dbo.Solution;");
        }

        public static async Task<SolutionEntity> GetByIdAsync(string solutionId)
        {
            return (await FetchAllAsync().ConfigureAwait(false)).First(item => solutionId == item.Id);
        }
    }
}
