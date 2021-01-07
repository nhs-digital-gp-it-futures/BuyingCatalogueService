using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public sealed class SolutionUpdateImplementationTimescalesTests
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
        public async Task ShouldUpdateSolutionImplementationTimescales()
        {
            const string expected = "an implementation timescales description";

            var validationResult = await UpdateImplementationTimescalesAsync(_existingSolutionId, expected);

            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(r => r.UpdateImplementationTimescalesAsync(It.Is<IUpdateImplementationTimescalesRequest>(r =>
                r.SolutionId == _existingSolutionId
                && r.Description == expected
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateImplementationTimescalesAsync(_existingSolutionId, new string('a', 1101));

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["description"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateImplementationTimescalesAsync(It.IsAny<IUpdateImplementationTimescalesRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateImplementationTimescalesAsync(_invalidSolutionId, "A description"));
        }

        private async Task<ISimpleResult> UpdateImplementationTimescalesAsync(string solutionId, string url)
        {
            var validationResult = await _context.UpdateImplementationTimescalesHandler.Handle(
                    new UpdateImplementationTimescalesCommand(solutionId, url), new CancellationToken());

            return validationResult;
        }
    }
}
