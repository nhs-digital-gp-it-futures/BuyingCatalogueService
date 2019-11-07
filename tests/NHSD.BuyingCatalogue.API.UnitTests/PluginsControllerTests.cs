using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class PluginsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private PlugInsController _plugInsController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _plugInsController = new PlugInsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var pluginsViewModel = new UpdateSolutionPluginsViewModel();

            var validationModel = new UpdateSolutionPluginsValidationResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionPluginsCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionPluginsViewModel == pluginsViewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result = (await _plugInsController.UpdatePlugInsAsync(SolutionId, pluginsViewModel)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionPluginsCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionPluginsViewModel ==
                        pluginsViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var pluginsViewModel = new UpdateSolutionPluginsViewModel();

            var validationModel = new UpdateSolutionPluginsValidationResult()
            {
                Required = { "plugins-required" },
                MaxLength = { "plugins-detail" }
            };

            _mockMediator.Setup(m =>
                m.Send(
                    It.Is<UpdateSolutionPluginsCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionPluginsViewModel == pluginsViewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result = (await _plugInsController.UpdatePlugInsAsync(SolutionId, pluginsViewModel)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateSolutionPluginsResult).Required.Should().BeEquivalentTo(new[] {"plugins-required"});
            (result.Value as UpdateSolutionPluginsResult).MaxLength.Should().BeEquivalentTo(new[] { "plugins-detail" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionPluginsCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionPluginsViewModel ==
                        pluginsViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
