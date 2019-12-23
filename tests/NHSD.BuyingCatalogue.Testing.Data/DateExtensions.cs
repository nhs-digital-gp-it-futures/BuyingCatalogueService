using System;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class DateExtensions
    {
        public static async Task<double> SecondsFromNow(this DateTime datetime)
        {
            //sql servers internal clock may differ from ours (especially when using docker containers)
            var currentDateTime = (await SqlRunner.FetchAllAsync<DateTime>($@"SELECT GETDATE()").ConfigureAwait(false)).First();
            return Math.Abs(currentDateTime.Subtract(datetime).TotalSeconds);
        }
    }
}
