using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class ClientApplicationNativeMobileExtensions
    {
        public static bool IsMobileConnectionDetailsComplete(this IClientApplication clientApplication) =>
            clientApplication?.MobileConnectionDetails?.ConnectionType?.Any() == true ||
            !string.IsNullOrEmpty(clientApplication?.MobileConnectionDetails?.MinimumConnectionSpeed) ||
            !string.IsNullOrEmpty(clientApplication?.MobileConnectionDetails?.Description);

        public static bool IsMobileOperatingSystems(this IClientApplication clientApplication) =>
            clientApplication?.MobileOperatingSystems?.OperatingSystems?.Any() == true;

        public static bool IsNativeMobileComplete(this IClientApplication clientApplication) =>
            clientApplication.IsMobileOperatingSystems();
    }
}
