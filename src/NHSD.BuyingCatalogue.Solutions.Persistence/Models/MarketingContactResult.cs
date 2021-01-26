using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class MarketingContactResult : IMarketingContactResult
    {
        public int Id { get; init; }

        public string SolutionId { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Email { get; init; }

        public string PhoneNumber { get; init; }

        public string Department { get; init; }

        public DateTime LastUpdated { get; init; }
    }
}
