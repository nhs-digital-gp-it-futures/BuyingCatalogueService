using System;
using System.Collections.Generic;
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
            ,'{AboutUrl}'
            ,'{Features}'
            ,'{ClientApplication}'
            ,'{Hosting}'
            ,'{RoadMap}'
            ,'{RoadMapImageUrl}')";


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
    }
}
