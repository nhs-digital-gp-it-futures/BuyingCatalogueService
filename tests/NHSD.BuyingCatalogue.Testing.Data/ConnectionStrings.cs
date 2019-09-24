namespace NHSD.BuyingCatalogue.Testing.Data
{
    internal static class ConnectionStrings
    {
        internal const string SAPassword = "Your_password456";

        internal static readonly string Master = @"Server=localhost;Initial Catalog=master;Persist Security Info=False;User Id=sa;Password=" + SAPassword;

        internal static readonly string GPitFuturesSetup = @"Server=localhost;Initial Catalog={0};Persist Security Info=False;User Id=sa;Password=" + SAPassword;

        internal const string GPitFutures = @"Server=localhost;Initial Catalog={0};Persist Security Info=False;User Id=NHSD;Password=DisruptTheMarket1!";
    }
}
