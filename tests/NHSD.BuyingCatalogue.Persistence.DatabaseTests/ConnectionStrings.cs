using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal static class ConnectionStrings
    {
        internal const string Master = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Trusted_Connection=True;Integrated Security=SSPI;Persist Security Info=False;";

        internal const string GPitFutures = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Trusted_Connection=True;Integrated Security=SSPI;Persist Security Info=False;";
    }
}






