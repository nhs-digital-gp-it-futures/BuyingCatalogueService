using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SolutionDetailEntity : EntityBase
    {
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

        protected override string InsertSql => @"UPDATE dbo.Solution
   SET Features = @Features,
       ClientApplication = @ClientApplication,
       Hosting = @Hosting,
       ImplementationDetail = @ImplementationDetail,
       RoadMap = @RoadMap,
       IntegrationsUrl = @IntegrationsUrl,
       AboutUrl = @AboutUrl,
       Summary = @Summary,
       FullDescription = @FullDescription,
       LastUpdated = @LastUpdated,
       LastUpdatedBy = @LastUpdatedBy
 WHERE Id = @SolutionId;";

        public static async Task<IEnumerable<SolutionDetailEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<SolutionDetailEntity>(@"SELECT Id AS SolutionId,
       Features,
       ClientApplication,
       Hosting,
       ImplementationDetail,
       RoadMap,
       IntegrationsUrl,
       AboutUrl,
       Summary,
       FullDescription,
       LastUpdated,
       LastUpdatedBy
  FROM dbo.Solution;");
        }

        public async Task InsertAndSetCurrentForSolutionAsync()
        {
            await InsertAsync();
        }

        public static async Task<SolutionDetailEntity> GetBySolutionIdAsync(string solutionId)
        {
            return (await FetchAllAsync()).First(item => solutionId == item.SolutionId);
        }
    }
}
