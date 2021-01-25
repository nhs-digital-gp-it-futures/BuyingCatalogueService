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
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class NativeMobileAdditionalInformationControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private NativeMobileAdditionalInformationController nativeMobileAdditionalInformationController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            nativeMobileAdditionalInformationController = new NativeMobileAdditionalInformationController(mockMediator.Object);
        }

        [TestCase(null)]
        [TestCase("Some additional Information")]
        public async Task ShouldGetNativeMobileAdditionalInformation(string information)
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.NativeMobileAdditionalInformation == information));

            var response = await nativeMobileAdditionalInformationController.GetAdditionalInformationAsync(
                SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var result = response.Value as GetNativeMobileAdditionalInformationResult;

            Assert.NotNull(result);
            result.NativeMobileAdditionalInformation.Should().Be(information);

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var response = await nativeMobileAdditionalInformationController.GetAdditionalInformationAsync(
                SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var result = response.Value as GetNativeMobileAdditionalInformationResult;

            Assert.NotNull(result);
            result.NativeMobileAdditionalInformation.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var model = new UpdateNativeMobileAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.IsValid).Returns(true);

            Expression<Func<UpdateSolutionNativeMobileAdditionalInformationCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.AdditionalInformation == model.NativeMobileAdditionalInformation;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult.Object);

            var response = await nativeMobileAdditionalInformationController.UpdateAdditionalInformationAsync(
                SolutionId,
                model) as NoContentResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var model = new UpdateNativeMobileAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string>
            {
                { "additional-information", "maxLength" },
            });

            validationResult.Setup(s => s.IsValid).Returns(false);

            Expression<Func<UpdateSolutionNativeMobileAdditionalInformationCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.AdditionalInformation == model.NativeMobileAdditionalInformation;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult.Object);

            var response = await nativeMobileAdditionalInformationController.UpdateAdditionalInformationAsync(
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
    }
}
