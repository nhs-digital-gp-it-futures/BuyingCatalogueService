using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.BrowserBased
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

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("Plugins.Required").Value<bool>()
                && JToken.Parse(r.ClientApplication).SelectToken("Plugins.AdditionalInformation").Value<string>() == "This is some information";

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));
            Context.MockSolutionRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdatePluginsAndNothingElse()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' } }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("Plugins.Required")?.Value<bool>().Should().BeTrue();
                json.SelectToken("Plugins.AdditionalInformation")?.Value<string>().Should().Contain("lorem ipsum");
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdatePlugins("yes", "lorem ipsum");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdatePluginsNullAdditionalInformation()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' } }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("Plugins.Required")?.Value<bool>().Should().BeTrue();
                json.SelectToken("Plugins.AdditionalInformation").Should().BeNullOrEmpty();
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdatePlugins("yes");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidPlugins()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdatePlugins(null, new string('a', 501));

            validationResult.IsValid.Should().Be(false);

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(2);
            results["plugins-required"].Should().Be("required");
            results["plugins-detail"].Should().Be("maxLength");

            Context.MockSolutionRepository.Verify(
                r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never());

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        [Test]
        public void ShouldThrowWhenNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdatePlugins("yes", "This is some information"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdatePlugins(string required = null, string additionalInformation = null)
        {
            var data = Mock.Of<IUpdateBrowserBasedPluginsData>(p =>
                p.Required == required && p.AdditionalInformation == additionalInformation);

            return await Context.UpdateSolutionPluginsHandler.Handle(
                new UpdateSolutionPluginsCommand(SolutionId, data),
                CancellationToken.None);
        }
    }
}
