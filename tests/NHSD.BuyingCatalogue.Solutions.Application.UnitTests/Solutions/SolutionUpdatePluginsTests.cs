using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdatePluginsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdatePlugins()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdatePlugins("yes", "This is some information");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("Plugins.Required").Value<bool>() == true
                && JToken.Parse(r.ClientApplication).SelectToken("Plugins.AdditionalInformation").Value<string>() == "This is some information"
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdatePluginsAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'orem ipsum' } }");

            var calledBack = false;
            
            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.SelectToken("Plugins.Required").Value<bool>().Should().BeTrue();
                    json.SelectToken("Plugins.AdditionalInformation").Value<string>().Should().Contain("orem ipsum");
                });

            var validationResult = await UpdatePlugins("yes", "orem ipsum");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidPlugins()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdatePlugins(null, new string('a', 501));
            validationResult.IsValid.Should().Be(false);
            validationResult.Required.Should().BeEquivalentTo(new [] { "plugins-required" });
            validationResult.MaxLength.Should().BeEquivalentTo(new[] { "plugins-detail" });

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void ShouldThrowWhenNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdatePlugins("yes", "This is some information"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<UpdateSolutionPluginsValidationResult> UpdatePlugins(string required = null, string additionalInformation = null)
        {
            return await Context.UpdateSolutionPluginsHandler.Handle(new UpdateSolutionPluginsCommand(SolutionId, new UpdateSolutionPluginsViewModel()
            {
                Required = required,
                AdditionalInformation = additionalInformation
            }), CancellationToken.None);
        }
    }
}
