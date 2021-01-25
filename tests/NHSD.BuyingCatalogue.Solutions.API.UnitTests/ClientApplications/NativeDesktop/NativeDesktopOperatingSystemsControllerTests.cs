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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class NativeDesktopOperatingSystemsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private NativeDesktopOperatingSystemsController desktopOperatingSystemsController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            desktopOperatingSystemsController = new NativeDesktopOperatingSystemsController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetNativeDesktopOperatingSystemsDescription()
        {
            const string description = "A description full of detail.";

            mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopOperatingSystemsDescription == description));

            var response = await desktopOperatingSystemsController.GetSupportedOperatingSystems(SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var result = response.Value as GetNativeDesktopOperatingSystemsResult;

            Assert.NotNull(result);
            result.OperatingSystemsDescription.Should().Be(description);

            mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var response = await desktopOperatingSystemsController.GetSupportedOperatingSystems(SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var result = response.Value as GetNativeDesktopOperatingSystemsResult;

            Assert.NotNull(result);
            result.OperatingSystemsDescription.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var model = new UpdateNativeDesktopOperatingSystemsViewModel
            {
                NativeDesktopOperatingSystemsDescription = "new description",
            };

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            Expression<Func<UpdateSolutionNativeDesktopOperatingSystemsCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.NativeDesktopOperatingSystemsDescription == model.NativeDesktopOperatingSystemsDescription;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await desktopOperatingSystemsController.UpdatedSupportedOperatingSystems(
                SolutionId,
                model) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var model = new UpdateNativeDesktopOperatingSystemsViewModel
            {
                NativeDesktopOperatingSystemsDescription = "new description",
            };

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string>
            {
                { "operating-systems-description", "required" },
            });

            validationModel.Setup(s => s.IsValid).Returns(false);

            Expression<Func<UpdateSolutionNativeDesktopOperatingSystemsCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.NativeDesktopOperatingSystemsDescription == model.NativeDesktopOperatingSystemsDescription;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await desktopOperatingSystemsController.UpdatedSupportedOperatingSystems(
                SolutionId,
                model) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
