using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Hostings
{
    [TestFixture]
    class GetHostingBySolutionIdTests
    {
        private TestContext _context;
        private string _solutionId;
        private CancellationToken _cancellationToken;
        private IHostingResult _hostingResult;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _solutionId = "Sln1";
            _cancellationToken = new CancellationToken();
            _context.MockSolutionDetailRepository
                .Setup(r => r.GetHostingBySolutionIdAsync(_solutionId, _cancellationToken))
                .ReturnsAsync(() => _hostingResult);
        }

        [Test]
        public async Task ShouldGetHostingById()
        {
            _hostingResult = Mock.Of<IHostingResult>(r =>
                r.Id == _solutionId &&
                r.Hosting == "{ 'PublicCloud': { 'Summary': 'Some summary', 'URL': 'www.somelink.com', 'ConnectivityRequired': 'This Solution requires a HSCN/N3 connection' } }"
                );

            var hosting = await _context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);

            hosting.Should().NotBeNull();
            hosting.PublicCloud.Should().NotBeNull();

            hosting.PublicCloud.Summary.Should().Be("Some summary");
            hosting.PublicCloud.URL.Should().Be("www.somelink.com");
            hosting.PublicCloud.ConnectivityRequired.Should().Be("This Solution requires a HSCN/N3 connection");
        }

        [Test]
        public async Task EmptyHostingResultReturnsDefaultHosting()
        {
            _hostingResult = Mock.Of<IHostingResult>(r =>
                r.Id == _solutionId &&
                r.Hosting == null
                );

            var hosting = await _context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);
            hosting.Should().NotBeNull();
            hosting.Should().BeEquivalentTo(new HostingDto());
        }

        [Test]
        public void NullHostingResultThrowsNotFoundException()
        {
            _hostingResult = null;

            Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(_solutionId), _cancellationToken));
        }
    }
}
