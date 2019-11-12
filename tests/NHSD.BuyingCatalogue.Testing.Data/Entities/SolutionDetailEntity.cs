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

        public string RoadMapImageUrl { get; set; }

        public string AboutUrl { get; set; }

        public string Summary { get; set; }

        public string FullDescription { get; set; }

        public DateTime LastUpdated { get; set; }

        public Guid LastUpdatedBy { get; set; }

        protected override string InsertSql  => $@"
        INSERT INTO [dbo].[SolutionDetailEntity]
        ([Id]
        ,[SolutionId]
        ,[PublishedStatusId]        
        ,[Features]
        ,[ClientApplication]
        ,[Hosting]
        ,[ImplementationDetail]
        ,[RoadMap]
        ,[RoadMapImageUrl]
        ,[AboutUrl]
        ,[Summary]
        ,[FullDescription]
        ,[LastUpdated]
        ,[LastUpdatedBy])
        VALUES
            ('{Id}'          
            ,'{SolutionId}'
            ,'{PublishedStatusId}'
            ,{NullOrWrapQuotes(Features)}
            ,{NullOrWrapQuotes(ClientApplication)}
            ,{NullOrWrapQuotes(Hosting)}
            ,{NullOrWrapQuotes(ImplementationDetail)}
            ,{NullOrWrapQuotes(RoadMap)}
            ,{NullOrWrapQuotes(RoadMapImageUrl)}
            ,{NullOrWrapQuotes(AboutUrl)}
            ,{NullOrWrapQuotes(Summary)}
            ,{NullOrWrapQuotes(FullDescription)}
            ,'{LastUpdated}'
            ,'{LastUpdatedBy}')";


        public static async Task<IEnumerable<SolutionDetailEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<SolutionDetailEntity>($@"SELECT 
                           ([Id]
        ,[SolutionId]
        ,[PublishedStatusId]        
        ,[Features]
        ,[ClientApplication]
        ,[Hosting]
        ,[ImplementationDetail]
        ,[RoadMap]
        ,[RoadMapImageUrl]
        ,[AboutUrl]
        ,[Summary]
        ,[FullDescription]
        ,[LastUpdated]
        ,[LastUpdatedBy])
                            FROM MarketingDetail");
        }


        public static async Task<SolutionDetailEntity> GetBySolutionIdAsync(string solutionId)
        {
            return (await FetchAllAsync()).First(item => solutionId.Equals(item.SolutionId));
        }
    }
}
