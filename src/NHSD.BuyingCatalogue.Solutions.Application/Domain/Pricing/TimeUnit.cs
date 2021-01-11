using System;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class TimeUnit : Enumerator
    {
        public static readonly TimeUnit PerMonth = new(1, "month", "per month");
        public static readonly TimeUnit PerYear = new(2, "year", "per year");

        public string Description { get; }

        private TimeUnit(int id, string name, string description) : base(id, name)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
