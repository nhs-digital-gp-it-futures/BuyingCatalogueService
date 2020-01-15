using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
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
    }
}
