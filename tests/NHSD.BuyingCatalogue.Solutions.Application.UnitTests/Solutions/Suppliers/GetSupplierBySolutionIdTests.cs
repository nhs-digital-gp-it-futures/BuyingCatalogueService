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
            var origionalAboutSupplier = new Supplier
            {
                Description = "Some Description",
                Link = "Some Link"
            };

            _supplierResult = Mock.Of<ISupplierResult>(r =>
                    r.SolutionId == _solutionId &&
                    r.Description == origionalAboutSupplier.Description &&
                    r.Link == origionalAboutSupplier.Link
                    );

            var newSupplier = await _context.GetAboutSupplierBySolutionIdHandler.Handle(
                new GetSupplierBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);

            newSupplier.Should().BeEquivalentTo(origionalAboutSupplier);
        }

        [Test]
        public async Task EmptyAboutSupplierResultReturnsDefaultAboutSuppler()
        {
            _supplierResult = Mock.Of<ISupplierResult>(r =>
                r.SolutionId == _solutionId &&
                r.Description == null &&
                r.Link == null
                );

            var supplier = await _context.GetAboutSupplierBySolutionIdHandler.Handle(
                new GetSupplierBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);
            supplier.Should().NotBeNull();
            supplier.Should().BeEquivalentTo(new SupplierDto());
        }
    }
}
