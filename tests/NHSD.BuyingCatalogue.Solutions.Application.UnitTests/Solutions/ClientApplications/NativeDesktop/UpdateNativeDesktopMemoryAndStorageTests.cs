using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class UpdateNativeDesktopMemoryAndStorageTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";
        private const string MinimumMemoryToken = "NativeDesktopMemoryAndStorage.MinimumMemoryRequirement";
        private const string StorageRequirementsToken = "NativeDesktopMemoryAndStorage.StorageRequirementsDescription";
        private const string MinimumCpuToken = "NativeDesktopMemoryAndStorage.MinimumCpu";
        private const string RecommendedResolutionToken = "NativeDesktopMemoryAndStorage.RecommendedResolution";

        private Mock<IUpdateNativeDesktopMemoryAndStorageData> dataMock;
        private string minimumMemoryRequirement;
        private string storageRequirements;
        private string minimumCpu;
        private string recommendedResolution;

        [SetUp]
        public void Setup()
        {
            minimumMemoryRequirement = "1MB";
            storageRequirements = "A requirement";
            minimumCpu = "1Hz";
            recommendedResolution = "1x1";
            dataMock = new Mock<IUpdateNativeDesktopMemoryAndStorageData>();
            dataMock.Setup(d => d.MinimumMemoryRequirement).Returns(() => minimumMemoryRequirement);
            dataMock.Setup(d => d.StorageRequirementsDescription).Returns(() => storageRequirements);
            dataMock.Setup(d => d.MinimumCpu).Returns(() => minimumCpu);
            dataMock.Setup(d => d.RecommendedResolution).Returns(() => recommendedResolution);
        }

        [Test]
        public async Task ValidDataShouldUpdateNativeDesktopMemoryAndStorage()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(string.Empty);
            var validationResult = await UpdateNativeDesktopMemoryAndStorage();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken(MinimumMemoryToken).Value<string>() == minimumMemoryRequirement
                && JToken.Parse(r.ClientApplication).SelectToken(StorageRequirementsToken).Value<string>() == storageRequirements
                && JToken.Parse(r.ClientApplication).SelectToken(MinimumCpuToken).Value<string>() == minimumCpu
                && JToken.Parse(r.ClientApplication).SelectToken(RecommendedResolutionToken).Value<string>() == recommendedResolution;

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));

            validationResult.IsValid.Should().BeTrue();
        }

        [TestCase("", "", "")]
        [TestCase(" ", " ", " ")]
        [TestCase(null, null, null)]
        [TestCase("", " ", null)]
        public async Task MissingDataShouldReturnRequiredValidationResult(string memory, string storage, string cpu)
        {
            minimumMemoryRequirement = memory;
            storageRequirements = storage;
            minimumCpu = cpu;
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage();

            Expression<Func<ISolutionRepository, Task>> solutionRepositoryExpression = r => r.ByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(solutionRepositoryExpression, Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> solutionDetailRepositoryExpression = r =>
                r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(solutionDetailRepositoryExpression, Times.Never());

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();

            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;

            Assert.NotNull(reqMaxLengthResult);
            reqMaxLengthResult.Required.Should().BeEquivalentTo(
                "minimum-memory-requirement",
                "storage-requirements-description",
                "minimum-cpu");
        }

        [Test]
        public async Task SomeMissingDataShouldReturnRequiredValidationResult()
        {
            minimumMemoryRequirement = "";
            storageRequirements = "a requirement";
            minimumCpu = null;
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage();

            Expression<Func<ISolutionRepository, Task>> solutionRepositoryExpression = r => r.ByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(solutionRepositoryExpression, Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> solutionDetailRepositoryExpression = r =>
                r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(solutionDetailRepositoryExpression, Times.Never());

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();

            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;

            Assert.NotNull(reqMaxLengthResult);
            reqMaxLengthResult.Required.Should().BeEquivalentTo("minimum-memory-requirement", "minimum-cpu");
        }

        [Test]
        public async Task TooLongDataForStorageRequirementsShouldReturnMaxLengthValidationResult()
        {
            storageRequirements = new string('a', 301);
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage();

            Expression<Func<ISolutionRepository, Task>> solutionRepositoryExpression = r => r.ByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(solutionRepositoryExpression, Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> solutionDetailRepositoryExpression = r =>
                r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(solutionDetailRepositoryExpression, Times.Never());

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();

            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;

            Assert.NotNull(reqMaxLengthResult);
            reqMaxLengthResult.MaxLength.Should().BeEquivalentTo("storage-requirements-description");
        }

        [Test]
        public async Task TooLongDataForMinimumCpuShouldReturnMaxLengthValidationResult()
        {
            minimumCpu = new string('b', 301);
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage();

            Expression<Func<ISolutionRepository, Task>> solutionRepositoryExpression = r => r.ByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(solutionRepositoryExpression, Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> solutionDetailRepositoryExpression = r =>
                r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(solutionDetailRepositoryExpression, Times.Never());

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();

            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;

            Assert.NotNull(reqMaxLengthResult);
            reqMaxLengthResult.MaxLength.Should().BeEquivalentTo("minimum-cpu");
        }

        [Test]
        public async Task TooLongDataForMultipleFieldsShouldReturnMaxLengthValidationResult()
        {
            storageRequirements = new string('a', 301);
            minimumCpu = new string('b', 301);
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage();

            Expression<Func<ISolutionRepository, Task>> solutionRepositoryExpression = r => r.ByIdAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(solutionRepositoryExpression, Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> solutionDetailRepositoryExpression = r =>
                r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(solutionDetailRepositoryExpression, Times.Never());

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();

            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;

            Assert.NotNull(reqMaxLengthResult);
            reqMaxLengthResult.MaxLength.Should().BeEquivalentTo("storage-requirements-description", "minimum-cpu");
        }

        [Test]
        public void InvalidSolutionIdShouldThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await UpdateNativeDesktopMemoryAndStorage());

            Expression<Func<ISolutionRepository, Task>> solutionRepositoryExpression = r =>
                r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(solutionRepositoryExpression, Times.Once());

            Expression<Func<ISolutionDetailRepository, Task>> solutionDetailRepositoryExpression = r =>
                r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(solutionDetailRepositoryExpression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeDesktopMemoryAndStorage()
        {
            return await Context.UpdateNativeDesktopMemoryAndStorageHandler.Handle(
                new UpdateNativeDesktopMemoryAndStorageCommand(SolutionId, dataMock.Object),
                CancellationToken.None);
        }
    }
}
