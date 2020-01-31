using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public class SolutionUpdateIntegrationsTests
    {
        private TestContext _context;
        private string _existingSolutionId = "Sln1";
        private string _invalidSolutionId = "Sln123";

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();
            _context.MockSolutionRepository.Setup(x => x.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _context.MockSolutionRepository.Setup(x => x.CheckExists(_invalidSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateSolutionIntegrations()
        {
            var expected = "an integrations url";

            var validationResult = await UpdateIntegrationsAsync(_existingSolutionId, expected)
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(r => r.UpdateIntegrationsAsync(It.Is<IUpdateIntegrationsRequest>(r =>
                r.SolutionId == _existingSolutionId
                && r.Url == expected
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateIntegrationsAsync(_existingSolutionId, new string('a', 1001))
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["link"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateIntegrationsAsync(It.IsAny<IUpdateIntegrationsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateIntegrationsAsync(_invalidSolutionId, "A description"));
        }

        private async Task<ISimpleResult> UpdateIntegrationsAsync(string solutionId, string url)
        {
            var validationResult = await _context.UpdateIntegrationsHandler.Handle(
                    new UpdateIntegrationsCommand(solutionId, url), new CancellationToken())
                .ConfigureAwait(false);
            return validationResult;
        }
    }
}
