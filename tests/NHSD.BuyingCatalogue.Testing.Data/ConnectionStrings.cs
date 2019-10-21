namespace NHSD.BuyingCatalogue.Testing.Data
{
    public static class ConnectionStrings
    {
        internal const string Master = @"Server="+ DataConstants.ServerNameForTests + ";Initial Catalog=master;Persist Security Info=False;User Id=sa;Password=" + DataConstants.SAPassword;

        internal const string GPitFuturesSetup = @"Server=" + DataConstants.ServerNameForTests + ";Initial Catalog=" + DataConstants.DatabaseName + ";Persist Security Info=False;User Id=sa;Password=" + DataConstants.SAPassword;

        internal const string GPitFutures = @"Server={0};Initial Catalog=" + DataConstants.DatabaseName + ";Persist Security Info=False;User Id=NHSD;Password=DisruptTheMarket1!";

        public static string ServiceConnectionString(string serverName = DataConstants.ServerNameForTests) => string.Format(GPitFutures, serverName);
    }
}
