using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeMobile
{
    [TestFixture]
    public sealed class MobileOperatingSystemsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private MobileOperatingSystemsController _mobileOperatingSystemsController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mobileOperatingSystemsController = new MobileOperatingSystemsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetOperatingSystems()
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MobileOperatingSystems == Mock.Of<IMobileOperatingSystems>(m =>
                        m.OperatingSystems == new HashSet<string>() {"Windows", "IOS"} &&
                        m.OperatingSystemsDescription == "desc")));

            var result =
                (await _mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var operatingSystem = result.Value as GetMobileOperatingSystemsResult;
            operatingSystem.OperatingSystems.Should().BeEquivalentTo(new[] {"Windows", "IOS"});
            operatingSystem.OperatingSystemsDescription.Should().Be("desc");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetOperatingSystemIsNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileOperatingSystems).Returns<IMobileOperatingSystems>(null);

            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(clientMock.Object);

            var result =
                (await _mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var operatingSystem = (result.Value as GetMobileOperatingSystemsResult);
            operatingSystem.OperatingSystems.Count().Should().Be(0);
            operatingSystem.OperatingSystemsDescription.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result =
                (await _mobileOperatingSystemsController.GetMobileOperatingSystems(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;
            
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var operatingSystem = (result.Value as GetMobileOperatingSystemsResult);
            operatingSystem.OperatingSystems.Count().Should().Be(0);
            operatingSystem.OperatingSystemsDescription.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionMobileOperatingSystemsViewModel();

            var validationModel = new RequiredMaxLengthResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel == viewModel), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel);

            var result =
                (await _mobileOperatingSystemsController.UpdateMobileOperatingSystems(SolutionId, viewModel)
                    .ConfigureAwait(false)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel == viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateSolutionMobileOperatingSystemsViewModel();

            var validationModel = new RequiredMaxLengthResult()
            {
                Required = { "operating-systems" },
                MaxLength = { "operating-systems-description" }
            };

            _mockMediator.Setup(m =>
                m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel == viewModel),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result = (await _mobileOperatingSystemsController.UpdateMobileOperatingSystems(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateFormRequiredMaxLengthResult).Required.Should().BeEquivalentTo(new[] { "operating-systems" });
            (result.Value as UpdateFormRequiredMaxLengthResult).MaxLength.Should().BeEquivalentTo(new[] { "operating-systems-description" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileOperatingSystemsCommand>(q =>
                        q.Id == SolutionId && q.ViewModel ==
                        viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
