using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class RoadMapGetByIdTests
    {
        private TestContext _context;
        private GetRoadMapBySolutionIdQuery _query;
        private CancellationToken _token;
        private const string _solutionId = "Sln1";
        private string _roadMapDescription = "Some roadmap description";
        private Mock<IRoadMapResult> _mockResult;
        private bool _solutionExists = true;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _query = new GetRoadMapBySolutionIdQuery(_solutionId);
            _token = new CancellationToken();

            _context.MockSolutionDetailRepository.Setup(r => r.GetRoadMapBySolutionIdAsync(_solutionId, _token))
                .ReturnsAsync(() => _mockResult.Object);
            _context.MockSolutionRepository.Setup(r => r.CheckExists(_solutionId, _token)).ReturnsAsync(() => _solutionExists);

            _mockResult = new Mock<IRoadMapResult>();
            _mockResult.Setup(m => m.Summary).Returns(() => _roadMapDescription);
        }

        [TestCase("Some description")]
        [TestCase("         ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldGetRoadMapDescription(string description)
        {
            _roadMapDescription = description;
            var result = await _context.GetRoadMapByIdHandler.Handle(_query, _token).ConfigureAwait(false);
            result.Summary.Should().Be(_roadMapDescription);
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            _solutionExists = false;
            var exception = Assert.ThrowsAsync<NotFoundException>( () => _context.GetRoadMapByIdHandler.Handle(_query, _token));

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_solutionId, _token), Times.Once);
            _context.MockSolutionRepository.VerifyNoOtherCalls();
            _context.MockSolutionDetailRepository.VerifyNoOtherCalls();

        }
    }
}
