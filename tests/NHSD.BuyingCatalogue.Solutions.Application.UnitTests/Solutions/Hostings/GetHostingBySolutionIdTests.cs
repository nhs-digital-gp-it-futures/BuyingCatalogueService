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
                r.Hosting == "{ 'PublicCloud': { 'Summary': 'Some public summary', 'URL': 'www.somepubliclink.com', 'ConnectivityRequired': 'This Solution requires a public HSCN/N3 connection' }, 'PrivateCloud': { 'Summary': 'Some private summary', 'Link': 'www.someprivatelink.com', 'RequiresHSCN': 'This Solution requires a private HSCN/N3 connection' }, 'HybridHostingType': { 'Summary': 'Some hybrid summary', 'Url': 'www.somehybridlink.com', 'HostingModel': 'Some hybrid hosting model', 'ConnectivityRequired': 'This Solution requires a hybrid HSCN/N3 connection' } }"
                );

            var hosting = await _context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);

            hosting.Should().NotBeNull();

            hosting.PublicCloud.Should().NotBeNull();
            hosting.PublicCloud.Summary.Should().Be("Some public summary");
            hosting.PublicCloud.URL.Should().Be("www.somepubliclink.com");
            hosting.PublicCloud.ConnectivityRequired.Should().Be("This Solution requires a public HSCN/N3 connection");

            hosting.PrivateCloud.Should().NotBeNull();
            hosting.PrivateCloud.Summary.Should().Be("Some private summary");
            hosting.PrivateCloud.Link.Should().Be("www.someprivatelink.com");
            hosting.PrivateCloud.HostingModel.Should().Be("Some private hosting model");
            hosting.PrivateCloud.RequiresHSCN.Should().Be("This Solution requires a private HSCN/N3 connection");

            hosting.HybridHostingType.Should().NotBeNull();
            hosting.HybridHostingType.Summary.Should().Be("Some hybrid summary");
            hosting.HybridHostingType.Url.Should().Be("www.somehybridlink.com");
            hosting.HybridHostingType.HostingModel.Should().Be("Some hybrid hosting model");
            hosting.HybridHostingType.ConnectivityRequired.Should().Be("This Solution requires a hybrid HSCN/N3 connection");
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
