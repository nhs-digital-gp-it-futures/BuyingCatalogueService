using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateFeaturesTests
    {
        private const string SolutionId = "Sln1";

        private TestContext context;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolutionFeatures()
        {
            var listing = new List<string> { "sheep", "cow", "donkey" };

            var validationResult = await UpdateSolutionFeaturesAsync(listing);

            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionFeaturesRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && r.Features == JsonConvert.SerializeObject(listing);

            context.MockSolutionRepository.Verify(
                r => r.UpdateFeaturesAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateSolutionFeaturesAsync(new List<string> { new('a', 101), "test" });

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["listing-1"].Should().Be("maxLength");

            context.MockSolutionRepository.Verify(
                r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            context.MockSolutionRepository.Verify(
                r => r.UpdateFeaturesAsync(It.IsAny<IUpdateSolutionFeaturesRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Test]
        public async Task ShouldValidateMultipleMaxLength()
        {
            var validationResult = await UpdateSolutionFeaturesAsync(new List<string>
            {
                new('a', 101),
                "test",
                new('b', 200),
                "test",
                "test",
                new('c', 105),
            });

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(3);
            results["listing-1"].Should().Be("maxLength");
            results["listing-3"].Should().Be("maxLength");
            results["listing-6"].Should().Be("maxLength");

            context.MockSolutionRepository.Verify(
                r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            context.MockSolutionRepository.Verify(
                r => r.UpdateFeaturesAsync(It.IsAny<IUpdateSolutionFeaturesRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var listing = new List<string> { "sheep", "cow", "donkey" };

            Task UpdateFeatures()
            {
                return context.UpdateSolutionFeaturesHandler.Handle(
                    new UpdateSolutionFeaturesCommand(SolutionId, new UpdateSolutionFeaturesViewModel { Listing = listing }),
                    CancellationToken.None);
            }

            Assert.ThrowsAsync<NotFoundException>(UpdateFeatures);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            context.MockSolutionRepository.Verify(
                r => r.UpdateFeaturesAsync(It.IsAny<IUpdateSolutionFeaturesRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        private async Task<ISimpleResult> UpdateSolutionFeaturesAsync(IEnumerable<string> listing)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns(SolutionId);

            context.MockSolutionRepository
                .Setup(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);

            var validationResult = await context.UpdateSolutionFeaturesHandler.Handle(
                new UpdateSolutionFeaturesCommand(SolutionId, new UpdateSolutionFeaturesViewModel { Listing = listing }),
                CancellationToken.None);

            return validationResult;
        }
    }
}
