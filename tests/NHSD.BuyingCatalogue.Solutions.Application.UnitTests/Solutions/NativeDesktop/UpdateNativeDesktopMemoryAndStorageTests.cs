using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.NativeDesktop
{
    [TestFixture]
    internal sealed class UpdateNativeDesktopMemoryAndStorageTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";
        private const string MinimumMemoryToken = "NativeDesktopMemoryAndStorage.MinimumMemoryRequirement";
        private const string StorageRequirementsToken = "NativeDesktopMemoryAndStorage.StorageRequirementsDescription";
        private const string MinimumCpuToken = "NativeDesktopMemoryAndStorage.MinimumCpu";
        private const string RecommendedResolutionToken = "NativeDesktopMemoryAndStorage.RecommendedResolution";

        private Mock<IUpdateNativeDesktopMemoryAndStorageData> _trimmedDataMock;
        private Mock<IUpdateNativeDesktopMemoryAndStorageData> _dataMock;
        private string _minimumMemoryRequirement;
        private string _storageRequirements;
        private string _minimumCpu;
        private string _recommendedResolution;

        [SetUp]
        public void Setup()
        {
            _minimumMemoryRequirement = "1MB";
            _storageRequirements = "A requirement";
            _minimumCpu = "1Hz";
            _recommendedResolution = "1x1";
            _trimmedDataMock = new Mock<IUpdateNativeDesktopMemoryAndStorageData>();
            _trimmedDataMock.Setup(x => x.MinimumMemoryRequirement).Returns(() => _minimumMemoryRequirement);
            _trimmedDataMock.Setup(x => x.StorageRequirementsDescription).Returns(() => _storageRequirements);
            _trimmedDataMock.Setup(x => x.MinimumCpu).Returns(() => _minimumCpu);
            _trimmedDataMock.Setup(x => x.RecommendedResolution).Returns(() => _recommendedResolution);
            _dataMock = new Mock<IUpdateNativeDesktopMemoryAndStorageData>();
            _dataMock.Setup(x => x.Trim()).Returns(() => _trimmedDataMock.Object);
        }

        [Test]
        public async Task ValidDataShouldUpdateNativeDesktopMemoryAndStorage()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("");
            var validationResult = await UpdateNativeDesktopMemoryAndStorage().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once);
            Context.MockSolutionDetailRepository.Verify(x => x.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(
                c => 
                    c.SolutionId == SolutionId &&
                        JToken.Parse(c.ClientApplication).SelectToken(MinimumMemoryToken).Value<string>() == _minimumMemoryRequirement &&
                        JToken.Parse(c.ClientApplication).SelectToken(StorageRequirementsToken).Value<string>() == _storageRequirements &&
                        JToken.Parse(c.ClientApplication).SelectToken(MinimumCpuToken).Value<string>() == _minimumCpu &&
                        JToken.Parse(c.ClientApplication).SelectToken(RecommendedResolutionToken).Value<string>() == _recommendedResolution
                ), It.IsAny<CancellationToken>()), Times.Once);
            validationResult.IsValid.Should().BeTrue();
        }

        [TestCase("", "", "")]
        [TestCase(" ", " ", " ")]
        [TestCase(null, null, null)]
        [TestCase("", " ", null)]
        public async Task MissingDataShouldReturnRequiredValidationResult(string memory, string storage, string cpu)
        {
            _minimumMemoryRequirement = memory;
            _storageRequirements = storage;
            _minimumCpu = cpu;
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage().ConfigureAwait(false);
            
            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();
            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;
            reqMaxLengthResult.Required.Should().BeEquivalentTo("minimum-memory-requirement", "storage-requirements-description", "minimum-cpu");
        }

        [Test]
        public async Task SomeMissingDataShouldReturnRequiredValidationResult()
        {
            _minimumMemoryRequirement = "";
            _storageRequirements = "a requirement";
            _minimumCpu = null;
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();
            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;
            reqMaxLengthResult.Required.Should().BeEquivalentTo("minimum-memory-requirement", "minimum-cpu");
        }

        [Test]
        public async Task TooLongDataForStorageRequirementsShouldReturnMaxLengthValidationResult()
        {
            _storageRequirements = new string('a', 301);
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();
            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;
            reqMaxLengthResult.MaxLength.Should().BeEquivalentTo("storage-requirements-description");
        }

        [Test]
        public async Task TooLongDataForMinimumCpuShouldReturnMaxLengthValidationResult()
        {
            _minimumCpu = new string('b', 301);
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();
            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;
            reqMaxLengthResult.MaxLength.Should().BeEquivalentTo("minimum-cpu");
        }

        [Test]
        public async Task TooLongDataForMultipleFieldsShouldReturnMaxLengthValidationResult()
        {
            _storageRequirements = new string('a', 301);
            _minimumCpu = new string('b', 301);
            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdateNativeDesktopMemoryAndStorage().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<RequiredMaxLengthResult>();
            var reqMaxLengthResult = validationResult as RequiredMaxLengthResult;
            reqMaxLengthResult.MaxLength.Should().BeEquivalentTo("storage-requirements-description", "minimum-cpu");
        }

        [Test]
        public void InvalidSolutionIdShouldThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await UpdateNativeDesktopMemoryAndStorage().ConfigureAwait(false));
            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Once);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void CommandTrimsData()
        {
            var command = new UpdateNativeDesktopMemoryAndStorageCommand(SolutionId, _dataMock.Object);
            _dataMock.Verify(x => x.Trim(), Times.Once);
            command.Data.Should().Be(_trimmedDataMock.Object);
        }

        private async Task<ISimpleResult> UpdateNativeDesktopMemoryAndStorage()
        {
            return await Context.UpdateNativeDesktopMemoryAndStorageHandler.Handle(
                new UpdateNativeDesktopMemoryAndStorageCommand(SolutionId, _dataMock.Object),
                new CancellationToken()).ConfigureAwait(false);
        }
    }
}
