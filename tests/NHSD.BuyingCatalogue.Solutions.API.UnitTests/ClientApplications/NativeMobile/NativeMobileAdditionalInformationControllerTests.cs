using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    public sealed class NativeMobileAdditionalInformationControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private NativeMobileAdditionalInformationController _nativeMobileAdditionalInformationController;
        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _nativeMobileAdditionalInformationController = new NativeMobileAdditionalInformationController(_mockMediator.Object);
        }

        [TestCase(null)]
        [TestCase("Some additional Information")]
        public async Task ShouldGetNativeMobileAdditionalInformation(string information)
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.NativeMobileAdditionalInformation == information));

            var response = (await _nativeMobileAdditionalInformationController.GetAdditionalInformationAsync(SolutionId)
                .ConfigureAwait(false)) as ObjectResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.Value as GetNativeMobileAdditionalInformationResult;

            result.NativeMobileAdditionalInformation.Should().Be(information);
            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var response =
                (await _nativeMobileAdditionalInformationController.GetAdditionalInformationAsync(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var result = response.Value as GetNativeMobileAdditionalInformationResult;
            result.NativeMobileAdditionalInformation.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateNativeMobileAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.IsValid).Returns(true);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionNativeMobileAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation == viewModel.NativeMobileAdditionalInformation),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult.Object);

            var response = await _nativeMobileAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as NoContentResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionNativeMobileAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation ==
                        viewModel.NativeMobileAdditionalInformation), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateNativeMobileAdditionalInformationViewModel();

            var validationResult = new Mock<ISimpleResult>();
            validationResult.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "additional-information", "maxLength" } });
            validationResult.Setup(s => s.IsValid).Returns(false);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionNativeMobileAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation == viewModel.NativeMobileAdditionalInformation),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationResult.Object);

            var response = await _nativeMobileAdditionalInformationController.UpdateAdditionalInformationAsync(SolutionId, viewModel)
                .ConfigureAwait(false) as BadRequestObjectResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = response.Value as Dictionary<string, string>;
            result.Count.Should().Be(1);
            result["additional-information"].Should().Be("maxLength");

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionNativeMobileAdditionalInformationCommand>(q =>
                        q.SolutionId == SolutionId && q.AdditionalInformation ==
                        viewModel.NativeMobileAdditionalInformation), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
