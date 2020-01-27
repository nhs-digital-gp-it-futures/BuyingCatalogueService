using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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

        [SetUp]
        public async Task SetUp()
        {
            await Database.ClearAsync().ConfigureAwait(false);

            TestContext testContext = new TestContext();
            _supplierRepository = testContext.SupplierRepository;
        }

        [Test]
        public async Task ShouldGetSupplier()
        {
            await InsertSupplier().ConfigureAwait(false);

            var result = await _supplierRepository.GetSupplierBySolutionIdAsync(_solutionId, new CancellationToken())
                .ConfigureAwait(false);

            result.SolutionId.Should().Be(_solutionId);
            result.Description.Should().Be(_description);
            result.Link.Should().Be(_link);
        }

        [Test]
        public async Task IfSolutionDoesNotExistThenReturnNull()
        {
            var result = (await _supplierRepository.GetSupplierBySolutionIdAsync(_solutionId, new CancellationToken())
                .ConfigureAwait(false));

            result.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateSupplier()
        {
            await InsertSupplier().ConfigureAwait(false);

            var supplierRequest = Mock.Of<IUpdateSupplierRequest>(s =>
                s.SolutionId == _solutionId && s.Description == _newDescription && s.Link == _newLink);

            await _supplierRepository.UpdateSupplierAsync(supplierRequest, new CancellationToken()).ConfigureAwait(false);

            var supplier = await _supplierRepository.GetSupplierBySolutionIdAsync(_solutionId, new CancellationToken())
                .ConfigureAwait(false);
            supplier.SolutionId.Should().Be(_solutionId);
            supplier.Description.Should().Be(_newDescription);
            supplier.Link.Should().Be(_newLink);
        }

        [Test]
        public async Task ShouldThrowOnUpdateIfRequestIsNull()
        {
            await InsertSupplier().ConfigureAwait(false);

            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _supplierRepository.UpdateSupplierAsync(null, new CancellationToken()));
        }

        private async Task InsertSupplier()
        {
            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithSummary(_description)
                .WithSupplierUrl(_link)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithId(_solutionId)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }
    }
}
