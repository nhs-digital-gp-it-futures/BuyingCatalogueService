using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class MarketingDetailEntity : EntityBase
    {
        public string SolutionId { get; set; }

        public string AboutUrl { get; set; }

        public string Features { get; set; }

        public string ClientApplication { get; set; }

        public string Hosting { get; set; }

        public string RoadMap { get; set; }

        public string RoadMapImageUrl { get; set; }

        protected override string InsertSql  => $@"
        INSERT INTO [dbo].[MarketingDetail]
        ([SolutionId]
        ,[AboutUrl]
        ,[Features]
        ,[ClientApplication]
        ,[Hosting]
        ,[RoadMap]
        ,[RoadMapImageUrl])
        VALUES
            ('{SolutionId}'
            ,{NullOrWrapQuotes(AboutUrl)}
            ,{NullOrWrapQuotes(Features)}
            ,{NullOrWrapQuotes(ClientApplication)}
            ,{NullOrWrapQuotes(Hosting)}
            ,{NullOrWrapQuotes(RoadMap)}
            ,{NullOrWrapQuotes(RoadMapImageUrl)})";


        public static async Task<IEnumerable<MarketingDetailEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<MarketingDetailEntity>($@"SELECT 
                            [SolutionId]
                            ,[AboutUrl]
                            ,[Features]
                            ,[ClientApplication]
                            ,[Hosting]
                            ,[RoadMap]
                            ,[RoadMapImageUrl]
                            FROM MarketingDetail");
        }


        public static async Task<MarketingDetailEntity> GetBySolutionIdAsync(string solutionId)
        {
            return (await FetchAllAsync()).First(item => solutionId.Equals(item.SolutionId));
        }
    }
}
