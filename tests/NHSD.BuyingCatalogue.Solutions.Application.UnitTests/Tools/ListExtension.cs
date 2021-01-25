using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools
{
    internal static class ListExtension
    {
        internal static IEnumerable<string> ShouldContainOnly(
            this IEnumerable<string> values,
            IEnumerable<string> expectation)
        {
            if (expectation is null)
            {
                throw new ArgumentNullException(nameof(expectation));
            }

            return CheckCollection(values, l => l.Should().BeEquivalentTo(expectation, o => o.WithoutStrictOrdering()));
        }

        internal static IEnumerable<string> ShouldNotContain(this IEnumerable<string> values, string expectation)
        {
            return CheckCollection(values, l => l.Should().NotContain(expectation));
        }

        private static IEnumerable<string> CheckCollection(
            IEnumerable<string> collection,
            Action<IEnumerable<string>> assertion)
        {
            var items = collection?.ToList();
            assertion(items);

            return items;
        }
    }
}
