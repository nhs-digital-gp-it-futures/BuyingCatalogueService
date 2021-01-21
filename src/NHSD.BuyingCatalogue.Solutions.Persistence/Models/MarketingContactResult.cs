﻿using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
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

        public DateTime LastUpdated { get; }
    }
}
