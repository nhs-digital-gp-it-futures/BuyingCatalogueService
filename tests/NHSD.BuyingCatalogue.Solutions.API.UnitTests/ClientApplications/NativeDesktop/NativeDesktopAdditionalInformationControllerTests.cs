using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class NativeDesktopAdditionalInformationControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private NativeDesktopAdditionalInformationController nativeDesktopAdditionalInformationController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            nativeDesktopAdditionalInformationController = new NativeDesktopAdditionalInformationController(mockMediator.Object);
        }

        [TestCase(null)]
        [TestCase("Some additional Information")]
        [TestCase("")]
        [TestCase(" Some additional tabbed Information  ")]
        public async Task ShouldGetNativeDesktopAdditionalInformation(string information)
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopAdditionalInformation == information));

            var response = await nativeDesktopAdditionalInformationController.GetAsync(SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var result = response.Value as GetNativeDesktopAdditionalInformationResult;

            Assert.NotNull(result);
            result.NativeDesktopAdditionalInformation.Should().Be(information);

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldReturnNull()
        {
            var response = await nativeDesktopAdditionalInformationController.GetAsync(SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var result = response.Value as GetNativeDesktopAdditionalInformationResult;

            Assert.NotNull(result);
            result.NativeDesktopAdditionalInformation.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var model = new UpdateNativeDesktopAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult
                .Setup(s => s.ToDictionary())
                .Returns(new Dictionary<string, string> { { "additional-information", "maxLength" } });

            validationResult.Setup(s => s.IsValid).Returns(false);

            Expression<Func<UpdateNativeDesktopAdditionalInformationCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.AdditionalInformation == model.NativeDesktopAdditionalInformation;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult.Object);

            var response = await nativeDesktopAdditionalInformationController.UpdateAdditionalInformationAsync(
                    SolutionId,
                    model) as BadRequestObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var result = response.Value as Dictionary<string, string>;

            Assert.NotNull(result);
            result.Count.Should().Be(1);
            result["additional-information"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var model = new UpdateNativeDesktopAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.IsValid).Returns(true);

            Expression<Func<UpdateNativeDesktopAdditionalInformationCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.AdditionalInformation == model.NativeDesktopAdditionalInformation;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult.Object);

            var response = await nativeDesktopAdditionalInformationController.UpdateAdditionalInformationAsync(
                    SolutionId,
                    model) as NoContentResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
