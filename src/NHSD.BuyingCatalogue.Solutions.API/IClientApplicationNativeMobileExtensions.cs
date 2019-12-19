using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class IClientApplicationNativeMobileExtensions
    {
        public static bool IsMobileOperatingSystems(this IClientApplication clientApplication) =>
            clientApplication?.MobileOperatingSystems?.OperatingSystems?.Any() == true;

        public static bool IsNativeMobileComplete(this IClientApplication clientApplication) =>
            clientApplication.IsMobileOperatingSystems();
    }
}
