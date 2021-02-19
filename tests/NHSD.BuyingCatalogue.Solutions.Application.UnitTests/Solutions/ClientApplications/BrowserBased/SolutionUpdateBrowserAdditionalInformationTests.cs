using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateBrowserBasedAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class SolutionUpdateBrowserAdditionalInformationTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateBrowserAdditionInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowserAdditionalInformation("Some info");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("AdditionalInformation").Value<string>() == "Some info";

            Context.MockSolutionRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateBrowserAdditionalInformationToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("AdditionalInformation").Should().BeNullOrEmpty();
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateBrowserAdditionalInformation();
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateSolutionBrowserAdditionalInformationAndNothingElse()
        {
            const string clientAppJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' } }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientAppJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("AdditionalInformation")?.Value<string>().Should().Be("Updated Additional Info");
                json.ReadStringArray("ClientApplicationTypes")
                    .ShouldContainOnly(new List<string> { "browser-based", "native-mobile" });

                json.ReadStringArray("BrowsersSupported")
                    .ShouldContainOnly(new List<string> { "Mozilla Firefox", "Edge" });

                json.SelectToken("MobileResponsive")?.Value<bool>().Should().BeFalse();
                json.SelectToken("Plugins.Required")?.Value<bool>().Should().BeTrue();
                json.SelectToken("Plugins.AdditionalInformation")?.Value<string>().Should().Be("lorem ipsum");
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateBrowserAdditionalInformation("Updated Additional Info");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidBrowserAdditionalInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowserAdditionalInformation(new string('a', 501));
            validationResult.IsValid.Should().BeFalse();

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["additional-information"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateBrowserAdditionalInformation("New Additional Info"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateBrowserAdditionalInformation(string additionalInformation = null)
        {
            return await Context.UpdateSolutionBrowserAdditionalInformationHandler.Handle(
                new UpdateBrowserBasedAdditionalInformationCommand(SolutionId, additionalInformation),
                CancellationToken.None);
        }
    }
}
