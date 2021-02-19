using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionEntity : EntityBase
    {
        public string SolutionId { get; set; }

        public int PublishedStatusId { get; set; }

        public string Name { get; set; }

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

        protected override string InsertSql => @"

        IF EXISTS(select * from dbo.Solution where Id=@SolutionId)
            update dbo.Solution
                 
            SET   Version=@Version,
                Summary=@Summary,
                FullDescription=@FullDescription,
                Features=@Features,
                ClientApplication=@ClientApplication,
                Hosting=@Hosting,
                RoadMap=@RoadMap,
                IntegrationsUrl=@IntegrationsUrl,
                AboutUrl=@AboutUrl,
                ServiceLevelAgreement=@ServiceLevelAgreement,
                WorkOfPlan=@WorkOfPlan,
                ImplementationDetail=@ImplementationDetail,
                LastUpdated=@LastUpdated,
                LastUpdatedBy=@LastUpdatedBy

                where Id=@SolutionId
        ELSE

            INSERT INTO dbo.Solution
            (
                Id,
                Version,
                Summary,
                FullDescription,
                Features,
                ClientApplication,
                Hosting,
                RoadMap,
                IntegrationsUrl,
                AboutUrl,
                ServiceLevelAgreement,
                WorkOfPlan,
                ImplementationDetail,
                LastUpdated,
                LastUpdatedBy
            )
            VALUES
            (
                @SolutionId,
                @Version,
                @Summary,
                @FullDescription,
                @Features,
                @ClientApplication,
                @Hosting,
                @RoadMap,
                @IntegrationsUrl,
                @AboutUrl,
                @ServiceLevelAgreement,
                @WorkOfPlan,
                @ImplementationDetail,
                @LastUpdated,
                @LastUpdatedBy
            );";

        public static async Task<IEnumerable<SolutionEntity>> FetchAllAsync()
        {
            const string selectSql = @"
                SELECT s.Id as SolutionId,
                       c.[Name],
                       s.[Version],
                       s.Summary,
                       s.FullDescription,
                       s.Features,
                       s.ClientApplication,
                       s.Hosting,
                       s.ImplementationDetail,
                       s.RoadMap,
                       s.IntegrationsUrl,
                       s.AboutUrl,
                       s.ServiceLevelAgreement,
                       s.WorkOfPlan,
                       s.LastUpdated,
                       s.LastUpdatedBy
                  FROM dbo.Solution AS s
                       INNER JOIN dbo.CatalogueItem AS c
                               ON c.CatalogueItemId = s.Id;";

            return await SqlRunner.FetchAllAsync<SolutionEntity>(selectSql);
        }

        public static async Task<SolutionEntity> GetByIdAsync(string solutionId)
        {
            return (await FetchAllAsync()).First(item => solutionId == item.SolutionId);
        }

        public async Task InsertAndSetCurrentForSolutionAsync()
        {
            await InsertAsync();
        }
    }
}
