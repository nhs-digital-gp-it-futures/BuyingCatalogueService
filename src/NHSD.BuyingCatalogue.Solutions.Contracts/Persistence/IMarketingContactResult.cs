using System;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IMarketingContactResult
    {
        int Id { get; }

        string SolutionId { get; }

        string FirstName { get; }

        string LastName { get; }

        string Email { get; }

        string PhoneNumber { get; }

        string Department { get; }

        DateTime LastUpdated { get; }
    }
}
