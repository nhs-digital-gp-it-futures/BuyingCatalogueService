using System;

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
    }
}
