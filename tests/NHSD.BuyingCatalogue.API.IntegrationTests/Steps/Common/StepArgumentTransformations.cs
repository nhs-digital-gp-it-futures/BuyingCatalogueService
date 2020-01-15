using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    }
}
