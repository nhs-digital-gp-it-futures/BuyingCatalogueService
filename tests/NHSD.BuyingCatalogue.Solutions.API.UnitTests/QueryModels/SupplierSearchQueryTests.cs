using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.API.QueryModels;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.QueryModels
{
    [TestFixture]
    internal sealed class SupplierSearchQueryTests
    {
        [TestCase(null, PublishedStatus.Withdrawn, PublishedStatus.Withdrawn)]
        [TestCase(false, PublishedStatus.Draft, PublishedStatus.Draft)]
        [TestCase(true, PublishedStatus.Unpublished, PublishedStatus.Published)]
        public void SolutionPublicationStatus_ReturnsExpectedStatus(
            bool? hasPublishedItems,
            PublishedStatus assignedStatus,
            PublishedStatus expectedStatus)
        {
            var model = new SupplierSearchQuery
            {
                LimitToPublishedSolutions = hasPublishedItems,
                SolutionPublicationStatus = assignedStatus.ToString()
            };

            model.SolutionPublicationStatus.Should().Be(expectedStatus.ToString());
        }

        [Test]
        public void SolutionPublicationStatus_SolutionPublicationStatus_ReturnsTrimmedStatus()
        {
            var expectedPublicationStatus = PublishedStatus.Withdrawn.ToString();

            var model = new SupplierSearchQuery { SolutionPublicationStatus = $" {expectedPublicationStatus}\t" };

            model.SolutionPublicationStatus.Should().Be(expectedPublicationStatus);
        }
    }
}
