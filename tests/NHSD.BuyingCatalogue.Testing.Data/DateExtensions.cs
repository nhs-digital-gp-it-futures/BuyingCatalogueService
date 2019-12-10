using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class DateExtensions
    {
        public static async Task IsWithinTimespan(this DateTime lastUpdated, TimeSpan duration)
        {
            //sql servers internal clock may differ from ours (especially when using docker containers)
            var currentDateTime = (await SqlRunner.FetchAllAsync<DateTime>($@"SELECT GETDATE()").ConfigureAwait(false)).First();
            lastUpdated.Should().BeOnOrAfter(currentDateTime - duration);
            lastUpdated.Should().BeOnOrBefore(currentDateTime);
        }
    }
}
