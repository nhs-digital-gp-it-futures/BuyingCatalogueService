using System;
using FluentAssertions;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public static class SolutionLastUpdatedExtension
    {
        public static void IsWithinTimespan(this DateTime lastUpdated, TimeSpan duration)
        {
            var currentDateTime = DateTime.Now;

            lastUpdated.Should().BeOnOrAfter(currentDateTime - duration);
            lastUpdated.Should().BeOnOrBefore(currentDateTime);
        }
    }
}
