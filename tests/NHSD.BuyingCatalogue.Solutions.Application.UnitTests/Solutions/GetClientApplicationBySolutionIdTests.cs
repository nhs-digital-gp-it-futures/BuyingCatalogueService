using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    class GetClientApplicationBySolutionIdTests
    {
        private TestContext _context;
        private string _solutionId;
        private CancellationToken _cancellationToken;
        private IClientApplicationResult _clientApplicationResult;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _solutionId = "Sln1";
            _cancellationToken = new CancellationToken();
            _context.MockSolutionDetailRepository
                .Setup(r => r.GetClientApplicationBySolutionIdAsync(_solutionId, _cancellationToken))
                .ReturnsAsync(() => _clientApplicationResult);
        }

        [Test]
        public async Task ShouldGetClientApplicationById()
        {
            _clientApplicationResult = Mock.Of<IClientApplicationResult>(r =>
                r.Id == _solutionId &&
                r.ClientApplication == "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'orem ipsum' } }"
                );

            var clientApplication = await _context.GetClientApplicationBySolutionIdHandler.Handle(
                new GetClientApplicationBySolutionIdQuery(_solutionId), _cancellationToken);
            clientApplication.Should().NotBeNull();
            clientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            clientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            clientApplication.MobileResponsive.Should().BeTrue();
            clientApplication.Plugins.Required.Should().BeTrue();
            clientApplication.Plugins.AdditionalInformation.Should().Be("orem ipsum");
        }

        [Test]
        public async Task EmptyClientApplicationResultReturnsDefaultClientApplication()
        {
            _clientApplicationResult = Mock.Of<IClientApplicationResult>(r =>
                r.Id == _solutionId &&
                r.ClientApplication == null
                );

            var clientApplication = await _context.GetClientApplicationBySolutionIdHandler.Handle(
                new GetClientApplicationBySolutionIdQuery(_solutionId), _cancellationToken);
            clientApplication.Should().NotBeNull();
            clientApplication.Should().BeEquivalentTo(new ClientApplicationDto());
        }

        [Test]
        public void NullClientApplicationResultThrowsNotFoundException()
        {
            _clientApplicationResult = null;

            Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetClientApplicationBySolutionIdHandler.Handle(
                new GetClientApplicationBySolutionIdQuery(_solutionId), _cancellationToken));
        }
    }
}
