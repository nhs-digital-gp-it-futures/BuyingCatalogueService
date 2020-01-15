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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopAdditionalInformationControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private NativeDesktopAdditionalInformationController _nativeDesktopAdditionalInformationController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _nativeDesktopAdditionalInformationController = new NativeDesktopAdditionalInformationController(_mockMediator.Object);
        }

        [TestCase(null)]
        [TestCase("Some additional Information")]
        [TestCase("")]
        [TestCase(" Some additional tabbed Information  ")]
        public async Task ShouldGetNativeDesktopAdditionalInformation(string information)
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopAdditionalInformation == information));

            var response = (await _nativeDesktopAdditionalInformationController.GetAsync(SolutionId)
                .ConfigureAwait(false)) as ObjectResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.Value as GetNativeDesktopAdditionalInformationResult;

            result.NativeDesktopAdditionalInformation.Should().Be(information);
            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldReturnNull()
        {
            var response =
                (await _nativeDesktopAdditionalInformationController.GetAsync(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var result = response.Value as GetNativeDesktopAdditionalInformationResult;
            result.NativeDesktopAdditionalInformation.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateNativeDesktopAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.IsValid).Returns(true);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateNativeDesktopAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation == viewModel.NativeDesktopAdditionalInformation),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult.Object);

            var response = await _nativeDesktopAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as NoContentResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateNativeDesktopAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation ==
                        viewModel.NativeDesktopAdditionalInformation), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateNativeDesktopAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "additional-information", "maxLength" } });
            validationResult.Setup(s => s.IsValid).Returns(false);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateNativeDesktopAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation == viewModel.NativeDesktopAdditionalInformation),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult.Object);

            var response = await _nativeDesktopAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as BadRequestObjectResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = response.Value as Dictionary<string, string>;
            result.Count.Should().Be(1);
            result["additional-information"].Should().Be("maxLength");

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateNativeDesktopAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation ==
                        viewModel.NativeDesktopAdditionalInformation), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
