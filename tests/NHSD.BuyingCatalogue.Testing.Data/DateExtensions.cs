using System;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class DateExtensions
    {
        public static async Task<double> SecondsFromNow(this DateTime dateTime)
        {
            // SQL Server's internal clock may differ from ours (especially when using docker containers)
            // ReSharper disable once StringLiteralTypo (SQL Server function)
            var currentDateTime = (await SqlRunner.FetchAllAsync<DateTime>(@"SELECT GETDATE();")).First();
            return Math.Abs(currentDateTime.Subtract(dateTime).TotalSeconds);
        }
    }
}
