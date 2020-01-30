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
    internal sealed class GetIntegrationsBySolutionIdTests
    {
        private TestContext _context;
        private GetIntegrationsBySolutionIdQuery _query;
        private CancellationToken _token;
        private const string _solutionId = "Sln1";
        private string _integrationsUrl = "Some integrations url";
        private Mock<IIntegrationsResult> _mockResult;
        private bool _solutionExists = true;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _query = new GetIntegrationsBySolutionIdQuery(_solutionId);
            _token = new CancellationToken();

            _context.MockSolutionDetailRepository.Setup(r => r.GetIntegrationsBySolutionIdAsync(_solutionId, _token))
            .ReturnsAsync(() => _mockResult.Object);
            _context.MockSolutionRepository.Setup(r => r.CheckExists(_solutionId, _token)).ReturnsAsync(() => _solutionExists);

            _mockResult = new Mock<IIntegrationsResult>();
            _mockResult.Setup(m => m.IntegrationsUrl).Returns(() => _integrationsUrl);
        }

        [TestCase("Some url")]
        [TestCase("         ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldGetIntegrationsUrl(string url)
        {
            _integrationsUrl = url;
            var result = await _context.GetIntegrationsBySolutionIdHandler.Handle(_query, _token).ConfigureAwait(false);
            result.Url.Should().Be(_integrationsUrl);
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            _solutionExists = false;
            var exception = Assert.ThrowsAsync<NotFoundException>( () => _context.GetIntegrationsBySolutionIdHandler.Handle(_query, _token));

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_solutionId, _token), Times.Once);
            _context.MockSolutionRepository.VerifyNoOtherCalls();
            _context.MockSolutionDetailRepository.VerifyNoOtherCalls();

        }
    }
}
