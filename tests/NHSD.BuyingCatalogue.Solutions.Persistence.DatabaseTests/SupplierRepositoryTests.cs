using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public sealed class SupplierRepositoryTests
    {
        private const string SupplierId = "Sup 1";
        private const string SolutionId = "Sln1";
        private const string Description = "some description";
        private const string Link = "www.someLink.com";
        private const string NewDescription = "new description";
        private const string NewLink = "www.newLink.com";
        private const string SupplierName = "some supplier name";

        private const string SupplierAddress = " { \"line1\": \"123 Line 1\", \"line2\": \"Line 2\", "
            + "\"line3\": \"Line 3\", \"line4\": \"Line 4\", \"line5\": \"Line 5\", "
            + "\"city\": \"Some town\", \"county\": \"Some county\", "
            + "\"postcode\": \"LS15 1BS\", \"country\": \"Some country\" }";

        private const string SupplierContactFirstName = "Bill";
        private const string SupplierContactLastName = "Smith";
        private const string SupplierContactEmail = "billsmith@email.com";
        private const string SupplierContactPhoneNumber = "0123456789";

        private ISupplierRepository supplierRepository;

        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync();

            TestContext testContext = new TestContext();
            supplierRepository = testContext.SupplierRepository;
        }

        [Test]
        public async Task ShouldGetSolutionSupplier()
        {
            await InsertSupplier();

            var result = await supplierRepository.GetSupplierBySolutionIdAsync(SolutionId, CancellationToken.None);

            result.SolutionId.Should().Be(SolutionId);
            result.Summary.Should().Be(Description);
            result.Url.Should().Be(Link);
        }

        [Test]
        public async Task IfSolutionDoesNotExistThenReturnNull()
        {
            var result = await supplierRepository.GetSupplierBySolutionIdAsync(SolutionId, CancellationToken.None);

            result.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateSolutionSupplier()
        {
            await InsertSupplier();

            var supplierRequest = Mock.Of<IUpdateSupplierRequest>(s =>
                s.SolutionId == SolutionId && s.Description == NewDescription && s.Link == NewLink);

            await supplierRepository.UpdateSupplierAsync(supplierRequest, CancellationToken.None);

            var supplier = await supplierRepository.GetSupplierBySolutionIdAsync(SolutionId, CancellationToken.None);
            supplier.SolutionId.Should().Be(SolutionId);
            supplier.Summary.Should().Be(NewDescription);
            supplier.Url.Should().Be(NewLink);
        }

        [Test]
        public async Task ShouldThrowOnUpdateIfRequestIsNull()
        {
            await InsertSupplier();

            Assert.ThrowsAsync<ArgumentNullException>(() =>
                supplierRepository.UpdateSupplierAsync(null, CancellationToken.None));
        }

        [Test]
        public async Task ShouldGetSupplier()
        {
            await InsertSupplier();

            var result = await supplierRepository.GetSupplierById(SupplierId, CancellationToken.None);

            var expected = new
            {
                Id = SupplierId,
                Name = SupplierName,
                AddressLine1 = "123 Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                AddressLine4 = "Line 4",
                AddressLine5 = "Line 5",
                Town = "Some town",
                County = "Some county",
                Postcode = "LS15 1BS",
                Country = "Some country",
                HasAddress = true,
                PrimaryContactFirstName = SupplierContactFirstName,
                PrimaryContactLastName = SupplierContactLastName,
                PrimaryContactEmailAddress = SupplierContactEmail,
                PrimaryContactTelephone = SupplierContactPhoneNumber,
                HasContact = true,
            };

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ByNameAsync_NoMatchingName_ReturnsEmptySet()
        {
            await InsertSupplier();

            var suppliers = await supplierRepository.GetSuppliersByNameAsync(
                "Invisible",
                null,
                null,
                CancellationToken.None);

            suppliers.Should().BeEmpty();
        }

        [TestCase(null, null, null)]
        [TestCase("Supplier", null, null)]
        [TestCase("Supplier", null, CatalogueItemType.Solution)]
        [TestCase("Supplier", PublishedStatus.Published, null)]
        [TestCase("Supplier", PublishedStatus.Published, CatalogueItemType.Solution)]
        public async Task GetSuppliersByNameAsync_PositiveMatch_ReturnsExpectedSupplier(
            string supplierName,
            PublishedStatus? solutionPublicationStatus,
            CatalogueItemType? catalogueItemType)
        {
            await InsertSupplier();

            var suppliers = await supplierRepository.GetSuppliersByNameAsync(
                supplierName,
                solutionPublicationStatus,
                catalogueItemType,
                CancellationToken.None);

            suppliers.Should().BeEquivalentTo(new { Id = SupplierId, Name = SupplierName });
        }

        [TestCase("Supplier", PublishedStatus.Draft, null)]
        [TestCase("Supplier", null, CatalogueItemType.AdditionalService)]
        [TestCase("Invisible", PublishedStatus.Published, null)]
        [TestCase("Invisible", null, null)]
        [TestCase(null, PublishedStatus.Withdrawn, null)]
        public async Task GetSuppliersByNameAsync_NoMatchingSuppliers_ReturnsEmptySet(
            string supplierName,
            PublishedStatus? solutionPublicationStatus,
            CatalogueItemType? catalogueItemType)
        {
            await InsertSupplier();

            var suppliers = await supplierRepository.GetSuppliersByNameAsync(
                supplierName,
                solutionPublicationStatus,
                catalogueItemType,
                CancellationToken.None);

            suppliers.Should().BeEmpty();
        }

        private static async Task InsertSupplier(PublishedStatus solutionPublicationStatus = PublishedStatus.Published)
        {
            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .WithName(SupplierName)
                .WithSummary(Description)
                .WithSupplierUrl(Link)
                .WithAddress(SupplierAddress)
                .Build()
                .InsertAsync();

            await SupplierContactEntityBuilder.Create()
                .WithId(Guid.NewGuid())
                .WithSupplierId(SupplierId)
                .WithFirstName(SupplierContactFirstName)
                .WithLastName(SupplierContactLastName)
                .WithEmail(SupplierContactEmail)
                .WithPhoneNumber(SupplierContactPhoneNumber)
                .Build()
                .InsertAsync();

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(SolutionId)
                .WithName(SolutionId)
                .WithSupplierId(SupplierId)
                .WithPublishedStatusId((int)solutionPublicationStatus)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(SolutionId)
                .Build()
                .InsertAsync();
        }
    }
}
