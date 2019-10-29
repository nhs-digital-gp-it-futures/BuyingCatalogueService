using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionGetByIdTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldGetSolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns("Description");
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns("AboutUrl");
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.Summary.Should().Be("Summary");
            solution.OrganisationName.Should().Be("OrganisationName");
            solution.Description.Should().Be("Description");
            solution.AboutUrl.Should().Be("AboutUrl");
            solution.Features.Should().BeEquivalentTo(new [] {"Marmite", "Jam", "Marmelade"});
            solution.ClientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            solution.ClientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            solution.ClientApplication.MobileResponsive.Should().BeTrue();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetEmptySolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);
            existingSolution.Setup(s => s.OrganisationName).Returns((string)null);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();
            solution.ClientApplication.ClientApplicationTypes.Should().BeEmpty();
            solution.ClientApplication.BrowsersSupported.Should().BeEmpty();
            solution.ClientApplication.MobileResponsive.Should().BeNull();

            solution.OrganisationName.Should().BeNull();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");

            solution.Summary.Should().Be("Summary");
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();
            solution.ClientApplication.ClientApplicationTypes.Should().BeEmpty();
            solution.ClientApplication.BrowsersSupported.Should().BeEmpty();
            solution.ClientApplication.MobileResponsive.Should().BeNull();

            solution.OrganisationName.Should().Be("OrganisationName");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionWithMarketingDataById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEquivalentTo(new[] { "Marmite", "Jam", "Marmelade" });

            solution.OrganisationName.Should().Be("OrganisationName");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionWithClientApplicationById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();
            solution.ClientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            solution.ClientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            solution.ClientApplication.MobileResponsive.Should().BeTrue();

            solution.OrganisationName.Should().Be("OrganisationName");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
