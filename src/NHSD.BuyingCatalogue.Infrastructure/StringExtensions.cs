using System;

namespace NHSD.BuyingCatalogue.Infrastructure
{
    public static class StringExtensions
    {
        public static bool? ToBoolean(this string candidate) => candidate?.ToUpperInvariant() == "YES" ? true : (candidate?.ToUpperInvariant() == "NO" ? false : (bool?)null);

        public static string NullIfWhitespace(this string candidate) => String.IsNullOrWhiteSpace(candidate) ? null : candidate;
    }
}
