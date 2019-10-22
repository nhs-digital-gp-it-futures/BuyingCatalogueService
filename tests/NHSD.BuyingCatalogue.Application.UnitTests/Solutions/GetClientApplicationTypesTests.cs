using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetClientApplicationTypes;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class GetClientApplicationTypesTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldGetClientApplicationTypesById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns("Description");
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns("AboutUrl");
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-desktop' ] }");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var clientApplicationTypes = await _context.GetClientApplicationTypesResultHandler.Handle(new GetClientApplicationTypesQuery("Sln1"), new CancellationToken());

            clientApplicationTypes.ClientApplicationTypes.Should().BeEquivalentTo(new string[] { "browser-based", "native-desktop" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetEmptyClientApplicationTypesById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns("Description");
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns("AboutUrl");
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var clientApplicationTypes = await _context.GetClientApplicationTypesResultHandler.Handle(new GetClientApplicationTypesQuery("Sln1"), new CancellationToken());

            clientApplicationTypes.ClientApplicationTypes.Should().BeEmpty();
                
            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetClientApplicationTypesResultHandler.Handle(new GetClientApplicationTypesQuery("Sln1"), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
