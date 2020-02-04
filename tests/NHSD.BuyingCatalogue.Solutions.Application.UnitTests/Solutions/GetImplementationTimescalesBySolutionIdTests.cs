using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetImplementationTimescalesBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class GetImplementationTimescalesBySolutionIdTests
    {
        private TestContext _context;
        private GetImplementationTimescalesBySolutionIdQuery _query;
        private CancellationToken _cancellationToken;
        private const string _solutionId = "Sln1";
        private string _implementationTimescalesDescription = "Some implementation timescales description";
        private Mock<IImplementationTimescalesResult> _mockResult;
        private bool _solutionExists = true;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _query = new GetImplementationTimescalesBySolutionIdQuery(_solutionId);
            _cancellationToken = new CancellationToken();

            _context.MockSolutionDetailRepository.Setup(r => r.GetImplementationTimescalesBySolutionIdAsync(_solutionId, _cancellationToken))
            .ReturnsAsync(() => _mockResult.Object);
            _context.MockSolutionRepository.Setup(r => r.CheckExists(_solutionId, _cancellationToken)).ReturnsAsync(() => _solutionExists);

            _mockResult = new Mock<IImplementationTimescalesResult>();
            _mockResult.Setup(m => m.Description).Returns(() => _implementationTimescalesDescription);
        }

        [TestCase("Some description")]
        [TestCase("         ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldGetImplementationTimescalesDescription(string description)
        {
            _implementationTimescalesDescription = description;
            var result = await _context.GetImplementationTimescalesBySolutionIdHandler.Handle(_query, _cancellationToken).ConfigureAwait(false);
            result.Description.Should().Be(_implementationTimescalesDescription);
        }

        [Test]
        public async Task EmptyImplementationTimescalesResultReturnsDefaultImplementationTimescales()
        {
            _implementationTimescalesDescription = null;

            var implementationTimescales = await _context.GetImplementationTimescalesBySolutionIdHandler.Handle(
                new GetImplementationTimescalesBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);
            implementationTimescales.Should().NotBeNull();
            implementationTimescales.Should().BeEquivalentTo(new ImplementationTimescalesDto());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            _solutionExists = false;
            var exception = Assert.ThrowsAsync<NotFoundException>( () => _context.GetImplementationTimescalesBySolutionIdHandler.Handle(_query, _cancellationToken));

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_solutionId, _cancellationToken), Times.Once);
            _context.MockSolutionRepository.VerifyNoOtherCalls();
            _context.MockSolutionDetailRepository.VerifyNoOtherCalls();
        }
    }
}
