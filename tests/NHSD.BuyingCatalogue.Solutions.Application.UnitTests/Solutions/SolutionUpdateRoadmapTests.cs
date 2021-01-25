using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadMap;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateRoadMapTests
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
        public async Task ShouldUpdateSolutionRoadMap()
        {
            const string expected = "a description";

            var validationResult = await UpdateRoadMapAsync(ExistingSolutionId, expected);
            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            context.MockSolutionRepository.Verify(r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateRoadMapRequest, bool>> match = r =>
                r.SolutionId == ExistingSolutionId
                && r.Description == expected;

            context.MockSolutionDetailRepository.Verify(
                r => r.UpdateRoadMapAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateRoadMapAsync(ExistingSolutionId, new string('a', 1001));
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["summary"].Should().Be("maxLength");

            context.MockSolutionRepository.Verify(
                r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            context.MockSolutionDetailRepository.Verify(
                r => r.UpdateRoadMapAsync(It.IsAny<IUpdateRoadMapRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateRoadMapAsync(InvalidSolutionId, "A description"));
        }

        private async Task<ISimpleResult> UpdateRoadMapAsync(string solutionId, string description)
        {
            var validationResult = await context.UpdateRoadMapHandler.Handle(
                new UpdateRoadMapCommand(solutionId, description),
                CancellationToken.None);

            return validationResult;
        }
    }
}
