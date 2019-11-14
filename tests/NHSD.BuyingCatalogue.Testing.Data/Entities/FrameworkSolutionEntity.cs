using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class FrameworkSolutionEntity : EntityBase
    {
        public string FrameworkId { get; set; }

        public string SolutionId { get; set; }

        public bool IsFoundation { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[FrameworkSolutions]
        ([FrameworkId]
        ,[SolutionId]
        ,[IsFoundation])

        VALUES
            ('{FrameworkId}'
            ,'{SolutionId}'
            ,{ToOneZero(IsFoundation)})";

        public static async Task<IEnumerable<FrameworkSolutionEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<FrameworkSolutionEntity>($@"SELECT [FrameworkId]
                                  ,[SolutionId]
                                  ,[IsFoundation]");
        }
    }
}
