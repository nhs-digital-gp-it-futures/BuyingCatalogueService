using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal class SolutionUpdateConnectivityAndResolutionTests : ClientApplicationTestsBase
    {
        private string _solutionId = "Sln1";
        private UpdateSolutionConnectivityAndResolutionViewModel _viewModel;
        private UpdateSolutionConnectivityAndResolutionCommand _command;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void Setup()
        {
            _viewModel = new UpdateSolutionConnectivityAndResolutionViewModel
            {
                MinimumConnectionSpeed = "1GBps", MinimumDesktopResolution = "1x1"
            };

            _command = new UpdateSolutionConnectivityAndResolutionCommand(_solutionId, _viewModel);
            _cancellationToken = new CancellationToken();
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
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumConnectionSpeed") == _viewModel.MinimumConnectionSpeed
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumDesktopResolution") == _viewModel.MinimumDesktopResolution
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task EmptyValuesIsInvalidValidAndNotSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            _viewModel.MinimumConnectionSpeed = null;
            _viewModel.MinimumDesktopResolution = null;
            var validationResult = await Context.UpdateSolutionConnectivityAndResolutionHandler.Handle(_command, _cancellationToken).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            validationResult.Required.ShouldContain("minimum-connection-speed");
            validationResult.Required.ShouldContain("minimum-desktop-resolution");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            _command = new UpdateSolutionConnectivityAndResolutionCommand("IDon'tExist", _viewModel);
            Assert.ThrowsAsync<NotFoundException>(() => Context.UpdateSolutionConnectivityAndResolutionHandler.Handle(_command, _cancellationToken));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("IDon'tExist", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
