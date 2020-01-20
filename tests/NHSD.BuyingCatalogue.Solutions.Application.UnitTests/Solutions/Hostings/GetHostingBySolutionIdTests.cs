using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Hostings;
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
            var originalHosting = new Hosting
            {
                PublicCloud = new PublicCloud
                {
                    Summary = "Some Summary",
                    URL = "www.somelink.com",
                    ConnectivityRequired = "This Solution requires a HSCN/N3 connection"
                },
                PrivateCloud = new PrivateCloud
                {
                    Summary = "Private Summary",
                    Link = "www.privatelink.com",
                    HostingModel = "Hosting Model",
                    RequiresHSCN = "How much wood would a woodchuck chuck if a woodchuck could chuck wood?"
                },
                HybridHostingType = new HybridHostingType
                {
                    Summary = "Private Summary",
                    Url = "www.privatelink.com",
                    HostingModel = "Hosting Model",
                    ConnectivityRequired = "This Solution requires a HSCN/N3 connection"
                },
                OnPremise = new OnPremise
                {
                    Summary = "Private Summary",
                    Link = "www.privatelink.com",
                    HostingModel = "Hosting Model",
                    RequiresHSCN = "This Solution requires a HSCN/N3 connection"
                }
            };

            _hostingResult = Mock.Of<IHostingResult>(r =>
                r.Id == _solutionId &&
                r.Hosting == JsonConvert.SerializeObject(originalHosting)
                );

            var newHosting = await _context.GetHostingBySolutionIdHandler.Handle(
                new GetHostingBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);

            newHosting.Should().BeEquivalentTo(originalHosting);
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
