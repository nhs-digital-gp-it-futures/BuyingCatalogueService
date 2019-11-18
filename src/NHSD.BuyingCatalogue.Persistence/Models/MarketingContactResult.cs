using System;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Persistence.Models
{
    internal sealed class MarketingContactResult : IMarketingContactResult
    {
        public int Id { get; }
        public string SolutionId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string Department { get; }
    }
}
