using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class PluginsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private PlugInsController plugInsController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            plugInsController = new PlugInsController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionPluginsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [Test]
        public async Task ShouldGetPlugins()
        {
            Expression<Func<IPlugins, bool>> plugins = c =>
                c.Required == true
                && c.AdditionalInformation == "Additional Information";

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(s => s.Plugins == Mock.Of(plugins)));

            var result = await plugInsController.GetPlugInsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var plugin = result.Value as GetPlugInsResult;

            Assert.NotNull(plugin);
            plugin.PlugIns.Should().Be("Yes");
            plugin.AdditionalInformation.Should().Be("Additional Information");

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [TestCase(null, null, null)]
        [TestCase(true, "Yes", null)]
        [TestCase(true, "Yes", "info")]
        [TestCase(false, "No", null)]
        [TestCase(false, "No", "add info")]
        public async Task ShouldGetPluginRequired(bool? pluginRequired, string expectedPlugin, string additionalInfo)
        {
            Expression<Func<IPlugins, bool>> plugins = c =>
                c.Required == pluginRequired
                && c.AdditionalInformation == additionalInfo;

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(s => s.Plugins == Mock.Of(plugins)));

            var result = await plugInsController.GetPlugInsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var plugin = result.Value as GetPlugInsResult;

            Assert.NotNull(plugin);
            plugin.PlugIns.Should().Be(expectedPlugin);
            plugin.AdditionalInformation.Should().Be(additionalInfo);

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetPluginsIsNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.Plugins).Returns<IPlugins>(null);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientMock.Object);

            var result = await plugInsController.GetPlugInsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var plugin = result.Value as GetPlugInsResult;

            Assert.NotNull(plugin);
            plugin.AdditionalInformation.Should().BeNull();
            plugin.PlugIns.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await plugInsController.GetPlugInsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var plugin = result.Value as GetPlugInsResult;

            Assert.NotNull(plugin);
            plugin.AdditionalInformation.Should().BeNull();
            plugin.PlugIns.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedPluginsViewModel
            {
                Required = "yes",
                AdditionalInformation = "Some info",
            };

            await plugInsController.UpdatePlugInsAsync(SolutionId, request);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionPluginsCommand>(c => c.SolutionId == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            resultDictionary.Add("plugins-required", "required");
            resultDictionary.Add("plugins-detail", "maxLength");

            var request = new UpdateBrowserBasedPluginsViewModel();

            var result = await plugInsController.UpdatePlugInsAsync(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(2);
            validationResult["plugins-required"].Should().Be("required");
            validationResult["plugins-detail"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionPluginsCommand>(c => c.SolutionId == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }
    }
}
