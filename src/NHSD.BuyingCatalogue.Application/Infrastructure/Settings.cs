using Microsoft.Extensions.Configuration;

namespace NHSD.BuyingCatalogue.Application.Infrastructure
{
    /// <summary>
    /// Provides the settings for the application.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Database connection string to MS-SQL-SERVER database holding buying catalogue data.
        /// </summary>
        /// <param name="configuration">The configuration provider to retrieve the value of a setting.</param>
        /// <returns>The connection string for the Buying Catalogue.</returns>
        public static string BuyingCatalogueConnectionString(this IConfiguration configuration) => configuration["ConnectionStrings:BuyingCatalogue"];
        public static string Jwt_Authority(this IConfiguration config) => config["Jwt:Authority"];
        public static string Jwt_Audience(this IConfiguration config) => config["Jwt:Audience"];
        public static string Jwt_UserInfo(this IConfiguration config) => config["Jwt:UserInfo"];
        public static string Jwt_IssuerSigningKey(this IConfiguration config) => config["Jwt:IssuerSigningKey"];
    }
}
