using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Suppliers
{
    [TestFixture]
    public sealed class GetSupplierBySolutionIdTests
    {
        private TestContext _context;
        private string _solutionId;
        private CancellationToken _cancellationToken;
        private ISupplierResult _supplierResult;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _solutionId = "Sln1";
            _cancellationToken = new CancellationToken();
            _context.MockSupplierRepository
                .Setup(r => r.GetSupplierBySolutionIdAsync(_solutionId, _cancellationToken))
                .ReturnsAsync(() => _supplierResult);
        }
        [Test]
        public async Task ShouldGetAboutSupplierById()
        {
            var originalSupplier = new Supplier
            {
                Name = "Some name",
                Summary = "Some Summary",
                Url = "Some Url"
            };

            _supplierResult = Mock.Of<ISupplierResult>(r =>
                r.SolutionId == _solutionId &&
                r.Name == originalSupplier.Name &&
                r.Summary == originalSupplier.Summary &&
                r.Url == originalSupplier.Url
                );

            var newSupplier = await _context.GetSupplierBySolutionIdHandler.Handle(
                new GetSupplierBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);

            newSupplier.Should().BeEquivalentTo(originalSupplier);
        }

        [Test]
        public async Task EmptySupplierResultReturnsDefaultSuppler()
        {
            _supplierResult = Mock.Of<ISupplierResult>(r =>
                r.SolutionId == _solutionId &&
                r.Name == null &&
                r.Summary == null &&
                r.Url == null
                );

            var supplier = await _context.GetSupplierBySolutionIdHandler.Handle(
                new GetSupplierBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);
            supplier.Should().NotBeNull();
            supplier.Should().BeEquivalentTo(new SupplierDto());
        }
    }
}
