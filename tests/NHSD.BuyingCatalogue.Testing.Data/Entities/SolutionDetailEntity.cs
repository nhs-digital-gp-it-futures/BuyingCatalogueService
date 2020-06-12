using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionDetailEntity : EntityBase
    {
        public Guid Id { get; set; }

        public string SolutionId { get; set; }

        public int PublishedStatusId { get; set; }

        public string Features { get; set; }

        public string ClientApplication { get; set; }

        public string Hosting { get; set; }

        public string ImplementationDetail { get; set; }

        public string RoadMap { get; set; }

        public string IntegrationsUrl { get; set; }

        public string AboutUrl { get; set; }

        public string Summary { get; set; }

        public string FullDescription { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[SolutionDetail]
        ([Id]
        ,[SolutionId]
        ,[PublishedStatusId]
        ,[Features]
        ,[ClientApplication]
        ,[Hosting]
        ,[ImplementationDetail]
        ,[RoadMap]
        ,[IntegrationsUrl]
        ,[AboutUrl]
        ,[Summary]
        ,[FullDescription]
        ,[LastUpdated]
        ,[LastUpdatedBy])
        VALUES
            (@Id
            ,@SolutionId
            ,@PublishedStatusId
            ,@Features
            ,@ClientApplication
            ,@Hosting
            ,@ImplementationDetail
            ,@RoadMap
            ,@IntegrationsUrl
            ,@AboutUrl
            ,@Summary
            ,@FullDescription
            ,@LastUpdated
            ,@LastUpdatedBy)";


        public static async Task<IEnumerable<SolutionDetailEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<SolutionDetailEntity>($@"SELECT
                            --[Id],
                            Solution.Id AS [SolutionId]
                            ,CatalogueItem.[PublishedStatusId]
                            ,[Features]
                            ,[ClientApplication]
                            ,[Hosting]
                            ,[ImplementationDetail]
                            ,[RoadMap]
                            ,[IntegrationsUrl]
                            ,[AboutUrl]
                            ,[Summary]
                            ,[FullDescription]
                            ,[LastUpdated]
                            ,[LastUpdatedBy]
                            FROM Solution
							LEFT JOIN CatalogueItem ON Solution.Id = CatalogueItem.CatalogueItemId; ")
                .ConfigureAwait(false);
        }

        public async Task InsertAndSetCurrentForSolutionAsync()
        {
            await base.InsertAsync().ConfigureAwait(false);
            await SqlRunner.ExecuteAsync(ConnectionStrings.GPitFuturesSetup,
                $@"UPDATE Solution SET SolutionDetailId = @Id
                    WHERE Id = @SolutionId",
                this)
                .ConfigureAwait(false);
        }

        public static async Task<SolutionDetailEntity> GetBySolutionIdAsync(string solutionId)
        {
            return (await FetchAllAsync().ConfigureAwait(false)).First(item => solutionId == item.SolutionId);
        }
    }
}
