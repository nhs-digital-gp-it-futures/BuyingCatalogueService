using System;
using System.Linq.Expressions;
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
    internal sealed class SolutionUpdateIntegrationsTests
    {
        private const string ExistingSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln123";

        private TestContext context;

        [SetUp]
        public void Setup()
        {
            context = new TestContext();
            context.MockSolutionRepository
                .Setup(r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateSolutionIntegrations()
        {
            const string expected = "an integrations url";

            var validationResult = await UpdateIntegrationsAsync(ExistingSolutionId, expected);

            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            context.MockSolutionRepository.Verify(r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateIntegrationsRequest, bool>> match = r =>
                r.SolutionId == ExistingSolutionId
                && r.Url == expected;

            context.MockSolutionRepository.Verify(
                r => r.UpdateIntegrationsAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateIntegrationsAsync(ExistingSolutionId, new string('a', 1001));

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["link"].Should().Be("maxLength");

            context.MockSolutionRepository.Verify(
                r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            context.MockSolutionRepository.Verify(
                r => r.UpdateIntegrationsAsync(It.IsAny<IUpdateIntegrationsRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateIntegrationsAsync(InvalidSolutionId, "A description"));
        }

        private async Task<ISimpleResult> UpdateIntegrationsAsync(string solutionId, string url)
        {
            var validationResult = await context.UpdateIntegrationsHandler.Handle(
                new UpdateIntegrationsCommand(solutionId, url),
                CancellationToken.None);

            return validationResult;
        }
    }
}
