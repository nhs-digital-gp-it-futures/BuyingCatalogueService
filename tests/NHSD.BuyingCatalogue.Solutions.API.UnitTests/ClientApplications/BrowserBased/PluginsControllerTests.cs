using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
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
    public sealed class PluginsControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private PlugInsController _plugInsController;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _plugInsController = new PlugInsController(_mockMediator.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mockMediator.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionPluginsCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [Test]
        public async Task ShouldGetPlugins()
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(s =>
                    s.Plugins == Mock.Of<IPlugins>(c =>
                        c.Required == true && c.AdditionalInformation == "Additional Information")));

            var result = (await _plugInsController.GetPlugInsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var plugin = (result.Value as GetPlugInsResult);
            plugin.PlugIns.Should().Be("Yes");
            plugin.AdditionalInformation.Should().Be("Additional Information");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null, null)]
        [TestCase(true, "Yes", null)]
        [TestCase(true, "Yes", "info")]
        [TestCase(false, "No", null)]
        [TestCase(false, "No", "add info")]
        public async Task ShouldGetPluginRequired(bool? pluginRequired, string expectedPlugin, string additionalInfo)
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(s =>
                    s.Plugins == Mock.Of<IPlugins>(c =>
                        c.Required == pluginRequired && c.AdditionalInformation == additionalInfo)));

            var result = (await _plugInsController.GetPlugInsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var plugin = (result.Value as GetPlugInsResult);
            plugin.PlugIns.Should().Be(expectedPlugin);
            plugin.AdditionalInformation.Should().Be(additionalInfo);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetPluginsIsNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.Plugins).Returns<IPlugins>(null);

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientMock.Object);

            var result = (await _plugInsController.GetPlugInsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var plugin = (result.Value as GetPlugInsResult);
            plugin.AdditionalInformation.Should().BeNull();
            plugin.PlugIns.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await _plugInsController.GetPlugInsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var plugin = (result.Value as GetPlugInsResult);
            plugin.AdditionalInformation.Should().BeNull();
            plugin.PlugIns.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedPluginsViewModel()
            {
                Required = "yes",
                AdditionalInformation = "Some info"
            };

            var result = (await _plugInsController.UpdatePlugInsAsync(SolutionId, request).ConfigureAwait(false)) as NoContentResult;

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionPluginsCommand>(q =>
                        q.SolutionId == SolutionId && q.Data == request), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            _resultDictionary.Add("plugins-required", "required");
            _resultDictionary.Add("plugins-detail", "maxLength");

            var request = new UpdateBrowserBasedPluginsViewModel();

            var result =
                (await _plugInsController.UpdatePlugInsAsync(SolutionId, request).ConfigureAwait(false)) as
                BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(2);
            validationResult["plugins-required"].Should().Be("required");
            validationResult["plugins-detail"].Should().Be("maxLength");

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionPluginsCommand>(q =>
                        q.SolutionId == SolutionId && q.Data ==
                        request), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
