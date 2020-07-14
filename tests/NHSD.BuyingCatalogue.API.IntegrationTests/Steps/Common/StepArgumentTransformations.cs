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
        internal static CatalogueItemType? ParseCatalogueItemType(string nullString)
        {
            if (nullString is "NULL")
            {
                return null;
            }

            var parsedCatalogueItemType = Enum.TryParse<CatalogueItemType>(nullString, out var catalogueItemType);

            parsedCatalogueItemType.Should().BeTrue("Invalid parsed value");

            return catalogueItemType;
        }
    }
}
