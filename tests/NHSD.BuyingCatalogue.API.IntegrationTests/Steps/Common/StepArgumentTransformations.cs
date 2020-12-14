using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class StepArgumentTransformations
    {
        [StepArgumentTransformation]
        internal static List<string> TransformToListOfString(string commaSeparatedList) =>
            commaSeparatedList.Split(",").Select(t => t.Trim().Trim('"')).ToList();

        [StepArgumentTransformation]
        internal static DateTime ParseDateTimeString(string dateString) =>
            DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        [StepArgumentTransformation]
        internal static string ParseStringToNull(string nullString)
        {
            return nullString is "NULL" ? null : nullString;
        }

        [StepArgumentTransformation]
        internal static CatalogueItemType? ParseCatalogueItemType(string input) => ParseEnum<CatalogueItemType>(input);

        [StepArgumentTransformation]
        internal static PublishedStatus? ParsePublishedStatus(string input) => ParseEnum<PublishedStatus>(input);

        private static TEnum? ParseEnum<TEnum>(string input)
            where TEnum : struct, Enum
        {
            if (input.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                return null;

            var parsed = Enum.TryParse<TEnum>(input, out var value);
            parsed.Should().BeTrue("Invalid value for {0}", typeof(TEnum).Name);

            return value;
        }
    }
}
