using System;
using NHSD.BuyingCatalogue.Infrastructure.Properties;

namespace NHSD.BuyingCatalogue.Infrastructure
{
    public static class StringExtensions
    {
        public static bool? ToBoolean(this string candidate) =>
            candidate?.ToUpperInvariant() switch
                {
                    "YES" => true,
                    "NO" => false,
                    _ => null
                };


        public static string NullIfWhitespace(this string candidate) => String.IsNullOrWhiteSpace(candidate) ? null : candidate;

        public static string ThrowIfNullOrWhitespace(this string candidate)
            => String.IsNullOrWhiteSpace(candidate)
                ? throw new ArgumentException(Resources.NullStringMessage, nameof(candidate))
                : candidate;
    }
}
