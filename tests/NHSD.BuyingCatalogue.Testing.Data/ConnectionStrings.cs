namespace NHSD.BuyingCatalogue.Testing.Data
{
    internal static class ConnectionStrings
    {
        private const string ServerNameForTests = "localhost";

        internal const string SAPassword = "Your_password456";

        internal static readonly string Master = @"Server="+ ServerNameForTests + ";Initial Catalog=master;Persist Security Info=False;User Id=sa;Password=" + SAPassword;

        internal static readonly string GPitFuturesSetup = @"Server=" + ServerNameForTests + ";Initial Catalog={0};Persist Security Info=False;User Id=sa;Password=" + SAPassword;

        internal const string GPitFutures = @"Server={0};Initial Catalog={1};Persist Security Info=False;User Id=NHSD;Password=DisruptTheMarket1!";
    }
}
