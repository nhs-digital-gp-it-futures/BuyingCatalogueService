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
        private ISupplierRepository _supplierRepository;

        private readonly string _supplierId = "Sup 1";
        private readonly string _solutionId = "Sln1";

        private readonly string _description = "some description";
        private readonly string _link = "www.someLink.com";

        private readonly string _newDescription = "new description";
        private readonly string _newLink = "www.newLink.com";

        private readonly string _supplierName = "some supplier name";
        private readonly string _supplierAddress = " { \"line1\": \"123 Line 1\", \"line2\": \"Line 2\", \"line3\": \"Line 3\", \"line4\": \"Line 4\", \"line5\": \"Line 5\", \"city\": \"Some town\", \"county\": \"Some county\", \"postcode\": \"LS15 1BS\", \"country\": \"Some country\" }";

        private readonly string _supplierContactFirstName = "Bill";
        private readonly string _supplierContactLastName = "Smith";
        private readonly string _supplierContactEmail = "billsmith@email.com";
        private readonly string _supplierContactPhoneNumber = "0123456789";

        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync();

            TestContext testContext = new TestContext();
            _supplierRepository = testContext.SupplierRepository;
        }

        [Test]
        public async Task ShouldGetSolutionSupplier()
        {
            await InsertSupplier();

            var result = await _supplierRepository.GetSupplierBySolutionIdAsync(_solutionId, new CancellationToken());

            result.SolutionId.Should().Be(_solutionId);
            result.Summary.Should().Be(_description);
            result.Url.Should().Be(_link);
        }

        [Test]
        public async Task IfSolutionDoesNotExistThenReturnNull()
        {
            var result = (await _supplierRepository.GetSupplierBySolutionIdAsync(_solutionId, new CancellationToken()));

            result.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateSolutionSupplier()
        {
            await InsertSupplier();

            var supplierRequest = Mock.Of<IUpdateSupplierRequest>(s =>
                s.SolutionId == _solutionId && s.Description == _newDescription && s.Link == _newLink);

            await _supplierRepository.UpdateSupplierAsync(supplierRequest, new CancellationToken());

            var supplier = await _supplierRepository.GetSupplierBySolutionIdAsync(_solutionId, new CancellationToken());
            supplier.SolutionId.Should().Be(_solutionId);
            supplier.Summary.Should().Be(_newDescription);
            supplier.Url.Should().Be(_newLink);
        }

        [Test]
        public async Task ShouldThrowOnUpdateIfRequestIsNull()
        {
            await InsertSupplier();

            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _supplierRepository.UpdateSupplierAsync(null, new CancellationToken()));
        }

        [Test]
        public async Task ShouldGetSupplier()
        {
            await InsertSupplier();

            var result = await _supplierRepository.GetSupplierById(_supplierId, CancellationToken.None);

            var expected = new
            {
                Id = _supplierId,
                Name = _supplierName,
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
                PrimaryContactFirstName = _supplierContactFirstName,
                PrimaryContactLastName = _supplierContactLastName,
                PrimaryContactEmailAddress = _supplierContactEmail,
                PrimaryContactTelephone = _supplierContactPhoneNumber,
                HasContact = true,
            };

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ByNameAsync_NoMatchingName_ReturnsEmptySet()
        {
            await InsertSupplier();

            var suppliers = await _supplierRepository.GetSuppliersByNameAsync(
                "Invisible",
                null,
                CancellationToken.None);

            suppliers.Should().BeEmpty();
        }

        [TestCase(null, null)]
        [TestCase("Supplier", null)]
        [TestCase("Supplier", PublishedStatus.Published)]
        public async Task GetSuppliersByNameAsync_PositiveMatch_ReturnsExpectedSupplier(
            string supplierName,
            PublishedStatus? solutionPublicationStatus)
        {
            await InsertSupplier();

            var suppliers = await _supplierRepository.GetSuppliersByNameAsync(
                supplierName,
                solutionPublicationStatus,
                CancellationToken.None);

            suppliers.Should().BeEquivalentTo(new { Id = _supplierId, Name = _supplierName });
        }

        [TestCase("Supplier", PublishedStatus.Draft)]
        [TestCase("Invisible", PublishedStatus.Published)]
        [TestCase("Invisible", null)]
        [TestCase(null, PublishedStatus.Withdrawn)]
        public async Task GetSuppliersByNameAsync_NoMatchingSuppliers_ReturnsEmptySet(
            string supplierName,
            PublishedStatus? solutionPublicationStatus)
        {
            await InsertSupplier();

            var suppliers = await _supplierRepository.GetSuppliersByNameAsync(
                supplierName,
                solutionPublicationStatus,
                CancellationToken.None);

            suppliers.Should().BeEmpty();
        }

        private async Task InsertSupplier(PublishedStatus solutionPublicationStatus = PublishedStatus.Published)
        {
            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithName(_supplierName)
                .WithSummary(_description)
                .WithSupplierUrl(_link)
                .WithAddress(_supplierAddress)
                .Build()
                .InsertAsync();

            await SupplierContactEntityBuilder.Create()
                .WithId(Guid.NewGuid())
                .WithSupplierId(_supplierId)
                .WithFirstName(_supplierContactFirstName)
                .WithLastName(_supplierContactLastName)
                .WithEmail(_supplierContactEmail)
                .WithPhoneNumber(_supplierContactPhoneNumber)
                .Build()
                .InsertAsync();

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(_solutionId)
                .WithName(_solutionId)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId((int)solutionPublicationStatus)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(_solutionId)
                .Build()
                .InsertAsync();
        }
    }
}
