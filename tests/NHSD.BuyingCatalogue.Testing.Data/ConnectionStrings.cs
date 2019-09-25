namespace NHSD.BuyingCatalogue.Testing.Data
{
    internal static class ConnectionStrings
    {
        internal static readonly string Master = @"Server="+ DataConstants.ServerNameForTests + ";Initial Catalog=master;Persist Security Info=False;User Id=sa;Password=" + DataConstants.SAPassword;

        internal static readonly string GPitFuturesSetup = @"Server=" + DataConstants.ServerNameForTests + ";Initial Catalog=" + DataConstants.DatabaseName + ";Persist Security Info=False;User Id=sa;Password=" + DataConstants.SAPassword;

        internal const string GPitFutures = @"Server={0};Initial Catalog=" + DataConstants.DatabaseName + ";Persist Security Info=False;User Id=NHSD;Password=DisruptTheMarket1!";
    }
}
