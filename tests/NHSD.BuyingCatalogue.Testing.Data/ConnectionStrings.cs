using System.Globalization;

namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class ConnectionStrings
    {
        internal const string GPitFuturesSetup = @"Server=" + DataConstants.ServerNameForTests
            + ";Initial Catalog=" + DataConstants.DatabaseName
            + ";Persist Security Info=False;User Id=sa;Password=" + DataConstants.SaPassword;

        private const string GPitFutures = @"Server={0};Initial Catalog=" + DataConstants.DatabaseName
            + ";Persist Security Info=False;User Id=NHSD-BAPI;Password=DisruptTheMarket1!";

        public static string ServiceConnectionString(string serverName = DataConstants.ServerNameForTests) =>
            string.Format(CultureInfo.InvariantCulture, GPitFutures, serverName);
    }
}
