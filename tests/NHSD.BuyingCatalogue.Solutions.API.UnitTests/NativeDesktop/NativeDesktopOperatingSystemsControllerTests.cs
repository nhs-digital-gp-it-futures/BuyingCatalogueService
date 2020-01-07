using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopOperatingSystemsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private NativeDesktopOperatingSystemsController _desktopOperatingSystemsController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _desktopOperatingSystemsController = new NativeDesktopOperatingSystemsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateNativeDesktopOperatingSystemsViewModel { NativeDesktopOperatingSystemsDescription = "new description" };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionNativeDesktopOperatingSystemsCommand>(q => SolutionId == q.SolutionId &&
                        q.NativeDesktopOperatingSystemsDescription == viewModel.NativeDesktopOperatingSystemsDescription), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = (await (_desktopOperatingSystemsController).UpdatedSupportedOperatingSystems(SolutionId, viewModel)
                    .ConfigureAwait(false)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionNativeDesktopOperatingSystemsCommand>(q =>
                        q.SolutionId == SolutionId &&
                        q.NativeDesktopOperatingSystemsDescription == viewModel.NativeDesktopOperatingSystemsDescription),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateNativeDesktopOperatingSystemsViewModel { NativeDesktopOperatingSystemsDescription = "new description" };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "operating-systems-description", "required" } });
            validationModel.Setup(s => s.IsValid).Returns(false);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionNativeDesktopOperatingSystemsCommand>(q => SolutionId == q.SolutionId &&
                                                                                   q.NativeDesktopOperatingSystemsDescription == viewModel.NativeDesktopOperatingSystemsDescription), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = (await (_desktopOperatingSystemsController).UpdatedSupportedOperatingSystems(SolutionId, viewModel)
                .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionNativeDesktopOperatingSystemsCommand>(q =>
                        q.SolutionId == SolutionId &&
                        q.NativeDesktopOperatingSystemsDescription == viewModel.NativeDesktopOperatingSystemsDescription),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
