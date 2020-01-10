using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.NativeMobile
{
    [TestFixture]
    internal sealed class SolutionUpdateMemoryAndStorageTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateMemoryAndStorage()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMemoryAndStorage("1GB", "A description").ConfigureAwait(false);

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("MobileMemoryAndStorage.Description").Value<string>() == "A description"
                && JToken.Parse(r.ClientApplication).SelectToken("MobileMemoryAndStorage.MinimumMemoryRequirement").Value<string>() == "1GB"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldNotUpdateOverLengthDescription(bool isDescriptionValid)
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var description = isDescriptionValid ? "Some Description" : new string('a', 301);

            var validationResult = await UpdateMemoryAndStorage("1GB", description).ConfigureAwait(false);

            if (isDescriptionValid)
            {
                validationResult.IsValid.Should().Be(true);
                Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
            }
            else
            {
                validationResult.IsValid.Should().Be(false);
                var results = validationResult.ToDictionary();
                results.Count.Should().Be(1);
                results["storage-requirements-description"].Should().Be("maxLength");
                Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public async Task ShouldNotUpdateMissingDescription(string description)
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMemoryAndStorage("1GB", description).ConfigureAwait(false);

            validationResult.IsValid.Should().Be(false);
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["storage-requirements-description"].Should().Be("required");
            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public async Task ShouldNotUpdateMissingMinimumMemoryRequirement(string minimumMemoryRequirement)
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMemoryAndStorage(minimumMemoryRequirement, "description").ConfigureAwait(false);

            validationResult.IsValid.Should().Be(false);
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["minimum-memory-requirement"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotUpdateMissingBoth()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMemoryAndStorage(null, null).ConfigureAwait(false);

            validationResult.IsValid.Should().Be(false);
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(2);
            results["minimum-memory-requirement"].Should().Be("required");
            results["storage-requirements-description"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateMemoryAndStorage("1GB", "Desc"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Once);

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var command = new UpdateSolutionMobileMemoryStorageCommand("Sln1", "     1GB", "    description    ");
            command.MinimumMemoryRequirement.Should().Be("1GB");
            command.Description.Should().Be("description");
        }

        private async Task<ISimpleResult> UpdateMemoryAndStorage(string minimumMemoryRequirement, string description)
        {
            return await Context.UpdateSolutionMobileMemoryStorageHandler.Handle(
                new UpdateSolutionMobileMemoryStorageCommand(SolutionId, minimumMemoryRequirement, description), CancellationToken.None).ConfigureAwait(false);
        }
    }
}
