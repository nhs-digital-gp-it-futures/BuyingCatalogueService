using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class UpdateSolutionCapabilitiesTests
    {
        private const string ValidSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln123";

        private TestContext context;

        [SetUp]
        public void Setup()
        {
            context = new TestContext();
            context.MockSolutionRepository
                .Setup(r => r.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateCapabilitiesAsync()
        {
            var capabilityRefs = new HashSet<string> { "C1", "C2" };
            const int expectedCapabilityCount = 2;

            context.MockSolutionCapabilityRepository
                .Setup(c => c.GetMatchingCapabilitiesCountAsync(capabilityRefs, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCapabilityCount);

            var validationResult = await UpdateCapabilitiesAsync(ValidSolutionId, capabilityRefs);
            validationResult.IsValid.Should().BeTrue();

            context.MockSolutionRepository.Verify(r => r.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateCapabilityRequest, bool>> match = c =>
                c.SolutionId == ValidSolutionId
                && ReferenceEquals(c.NewCapabilitiesReference, capabilityRefs);

            context.MockSolutionCapabilityRepository.Verify(
                r => r.UpdateCapabilitiesAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldValidateCapabilities()
        {
            var capabilitiesToMatch = new HashSet<string> { "C2", "C3" };
            const int expectedCapabilityCount = 1;

            context.MockSolutionCapabilityRepository
                .Setup(c => c.GetMatchingCapabilitiesCountAsync(capabilitiesToMatch, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCapabilityCount);

            var validationResult = await UpdateCapabilitiesAsync(ValidSolutionId, capabilitiesToMatch);

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(expectedCapabilityCount);
            results["capabilities"].Should().Be("capabilityInvalid");

            context.MockSolutionCapabilityRepository.Verify(
                r => r.GetMatchingCapabilitiesCountAsync(capabilitiesToMatch, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateCapabilityRequest, bool>> match = c =>
                c.SolutionId == ValidSolutionId
                && ReferenceEquals(c.NewCapabilitiesReference, capabilitiesToMatch);

            context.MockSolutionCapabilityRepository.Verify(
                r => r.UpdateCapabilitiesAsync(It.Is(match), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateCapabilitiesAsync(InvalidSolutionId, new HashSet<string>()));
        }

        private async Task<ISimpleResult> UpdateCapabilitiesAsync(string solutionId, HashSet<string> capabilityRefs)
        {
            return await context.UpdateCapabilitiesHandler.Handle(
                new UpdateCapabilitiesCommand(solutionId, capabilityRefs),
                CancellationToken.None);
        }
    }
}
