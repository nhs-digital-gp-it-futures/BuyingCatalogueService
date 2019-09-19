namespace NHSD.BuyingCatalogue.Testing.Data
{
    internal static class ConnectionStrings
    {
        internal const string Master = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Trusted_Connection=True;Integrated Security=SSPI;Persist Security Info=False;";

        internal const string GPitFutures = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Trusted_Connection=True;Integrated Security=SSPI;Persist Security Info=False;";
    }
}
