using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.BrowserBased
{
    [TestFixture]
    internal class SolutionUpdateConnectivityAndResolutionTests : ClientApplicationTestsBase
    {
        private string _solutionId = "Sln1";
        private Mock<IUpdateBrowserBasedConnectivityAndResolutionData> _trimmedViewModel;
        private Mock<IUpdateBrowserBasedConnectivityAndResolutionData> _viewModel;
        private string _connectionSpeed;
        private string _minimumResolution;

        private UpdateSolutionConnectivityAndResolutionCommand _command;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void Setup()
        {
            _trimmedViewModel = new Mock<IUpdateBrowserBasedConnectivityAndResolutionData>();
            _trimmedViewModel.Setup(x => x.MinimumConnectionSpeed).Returns(() => _connectionSpeed);
            _trimmedViewModel.Setup(x => x.MinimumDesktopResolution).Returns(() => _minimumResolution);
            _viewModel = new Mock<IUpdateBrowserBasedConnectivityAndResolutionData>();
            _viewModel.Setup(x => x.Trim()).Returns(() => _trimmedViewModel.Object);
            _command = new UpdateSolutionConnectivityAndResolutionCommand(_solutionId, _viewModel.Object);
            _cancellationToken = new CancellationToken();
            _connectionSpeed = "1";
            _minimumResolution = "800x600";
        }

        [Test]
        public async Task ValidValuesAreValidAndSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            var validationResult = await Context.UpdateSolutionConnectivityAndResolutionHandler.Handle(_command, _cancellationToken).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == "Sln1"
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumConnectionSpeed") == _connectionSpeed
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumDesktopResolution") == _minimumResolution
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ValidOnlyConnectionSpeedAreValidAndSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            _minimumResolution = null;
            var validationResult = await Context.UpdateSolutionConnectivityAndResolutionHandler.Handle(_command, _cancellationToken).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == "Sln1"
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumConnectionSpeed") == _connectionSpeed
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumDesktopResolution") == _minimumResolution
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task EmptyValuesIsInvalidAndNotSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            _connectionSpeed= null;
            _minimumResolution = null;
            var validationResult = await Context.UpdateSolutionConnectivityAndResolutionHandler.Handle(_command, _cancellationToken).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            validationResult.ToDictionary()["minimum-connection-speed"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task EmptyConnectionSpeedIsInvalidAndNotSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            _connectionSpeed = null;
            var validationResult = await Context.UpdateSolutionConnectivityAndResolutionHandler.Handle(_command, _cancellationToken).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            validationResult.ToDictionary()["minimum-connection-speed"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            _command = new UpdateSolutionConnectivityAndResolutionCommand("IDon'tExist", _viewModel.Object);
            Assert.ThrowsAsync<NotFoundException>(() => Context.UpdateSolutionConnectivityAndResolutionHandler.Handle(_command, _cancellationToken));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("IDon'tExist", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var viewModel = new Mock<IUpdateBrowserBasedConnectivityAndResolutionData>();
            var trimmedViewModel = Mock.Of<IUpdateBrowserBasedConnectivityAndResolutionData>();
            viewModel.Setup(x => x.Trim()).Returns(trimmedViewModel);

            var command = new UpdateSolutionConnectivityAndResolutionCommand("Sln1", viewModel.Object);
            viewModel.Verify(x => x.Trim(), Times.Once);

            command.Data.IsSameOrEqualTo(trimmedViewModel);
        }
    }
}
