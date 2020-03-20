using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.BrowserBased
{
    [TestFixture]
    internal class SolutionUpdateConnectivityAndResolutionTests : ClientApplicationTestsBase
    {
        private string _solutionId = "Sln1";

        [Test]
        public async Task ValidValuesAreValidAndSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateConnectivityAndResult("New Speed", "New Resolution").ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == _solutionId
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumConnectionSpeed") == "New Speed"
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumDesktopResolution") == "New Resolution"
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ValidOnlyConnectionSpeedAreValidAndSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            var validationResult = await UpdateConnectivityAndResult("Speed").ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == _solutionId
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumConnectionSpeed") == "Speed"
                && JToken.Parse(r.ClientApplication).Value<string>("MinimumDesktopResolution") == null
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task EmptyValuesIsInvalidAndNotSentToDatabase()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            var validationResult = await UpdateConnectivityAndResult().ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            validationResult.ToDictionary()["minimum-connection-speed"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(_solutionId, It.IsAny<CancellationToken>()), Times.Never);

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateConnectivityAndResult("Speed", "Resolution"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(_solutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<ISimpleResult> UpdateConnectivityAndResult(string connectionSpeed = null,
            string resolution = null)
        {
            var data = Mock.Of<IUpdateBrowserBasedConnectivityAndResolutionData>(c =>
                c.MinimumConnectionSpeed == connectionSpeed && c.MinimumDesktopResolution == resolution);

            return await Context.UpdateSolutionConnectivityAndResolutionHandler
                .Handle(new UpdateSolutionConnectivityAndResolutionCommand(_solutionId, data), new CancellationToken())
                .ConfigureAwait(false);
        }
    }
}
