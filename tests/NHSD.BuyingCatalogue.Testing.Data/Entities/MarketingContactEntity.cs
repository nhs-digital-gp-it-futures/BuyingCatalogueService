using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public class MarketingContactEntity : EntityBase
    {
        public string SolutionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid LastUpdatedBy { get; set; }

        protected override string InsertSql => $@"
            INSERT INTO [dbo].[MarketingContact]
            ([SolutionId]
            ,[FirstName]
            ,[LastName]
            ,[Email]
            ,[PhoneNumber]
            ,[Department]
            ,[LastUpdated]
            ,[LastUpdatedBy])
            VALUES
            ('{SolutionId}'
            ,{NullOrWrapQuotes(FirstName)}
            ,{NullOrWrapQuotes(LastName)}
            ,{NullOrWrapQuotes(Email)}
            ,{NullOrWrapQuotes(PhoneNumber)}
            ,{NullOrWrapQuotes(Department)}
            ,'{LastUpdated.ToString("dd-MMM-yyyy")}'
            ,'{LastUpdatedBy}'
            )";
        
        public static async Task<IEnumerable<MarketingContactEntity>> FetchForSolutionAsync(string solutionId)
        {
            return await SqlRunner.FetchAllAsync<MarketingContactEntity>($@"SELECT
             [Id]
            ,[SolutionId]
            ,[FirstName]
            ,[LastName]
            ,[Email]
            ,[PhoneNumber]
            ,[Department]
            ,[LastUpdated]
            ,[LastUpdatedBy]
            FROM MarketingContact
            Where SolutionId = @solutionId",
            new {solutionId}
            );
        }

        public MarketingContactEntity SetLastUpdated()
        {
            LastUpdated = DateTime.Now;
            LastUpdatedBy = Guid.NewGuid();
            return this;
        }
    }
}
