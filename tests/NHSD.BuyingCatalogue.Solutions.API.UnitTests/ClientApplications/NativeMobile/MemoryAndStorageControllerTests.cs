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
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class MemoryAndStorageControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private MemoryAndStorageController memoryAndStorageController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            memoryAndStorageController = new MemoryAndStorageController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetMemoryAndStorage()
        {
            Expression<Func<IMobileMemoryAndStorage, bool>> mobileMemoryAndStorage = m =>
                m.MinimumMemoryRequirement == "1GB"
                && m.Description == "desc";

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.MobileMemoryAndStorage == Mock.Of(mobileMemoryAndStorage);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await memoryAndStorageController.GetMemoryAndStorageAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var memoryAndStorage = result.Value as GetSolutionMemoryAndStorageResult;

            Assert.NotNull(memoryAndStorage);
            memoryAndStorage.MinimumMemoryRequirement.Should().Be("1GB");
            memoryAndStorage.Description.Should().Be("desc");

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetMemoryAndStorageIsNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileOperatingSystems).Returns<IMobileMemoryAndStorage>(null);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientMock.Object);

            var result = await memoryAndStorageController.GetMemoryAndStorageAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var memoryAndStorage = result.Value as GetSolutionMemoryAndStorageResult;

            Assert.NotNull(memoryAndStorage);
            memoryAndStorage.MinimumMemoryRequirement.Should().BeNull();
            memoryAndStorage.Description.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await memoryAndStorageController.GetMemoryAndStorageAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var memoryAndStorage = result.Value as GetSolutionMemoryAndStorageResult;

            Assert.NotNull(memoryAndStorage);
            memoryAndStorage.MinimumMemoryRequirement.Should().BeNull();
            memoryAndStorage.Description.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var model = new UpdateSolutionMemoryAndStorageRequest();

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            Expression<Func<UpdateSolutionMobileMemoryStorageCommand, bool>> match = c =>
                c.Id == SolutionId
                && c.MinimumMemoryRequirement == model.MinimumMemoryRequirement
                && c.Description == model.Description;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await memoryAndStorageController.UpdateMemoryAndStorageAsync(
                SolutionId,
                model) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var model = new UpdateSolutionMemoryAndStorageRequest();

            var validationModel = new Mock<ISimpleResult>();
            validationModel
                .Setup(s => s.ToDictionary())
                .Returns(new Dictionary<string, string>
                {
                    { "minimum-memory-requirement", "required" },
                    { "storage-requirements-description", "maxLength" },
                });

            validationModel.Setup(s => s.IsValid).Returns(false);

            Expression<Func<UpdateSolutionMobileMemoryStorageCommand, bool>> match = c =>
                c.Id == SolutionId
                && c.MinimumMemoryRequirement == model.MinimumMemoryRequirement
                && c.Description == model.Description;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await memoryAndStorageController.UpdateMemoryAndStorageAsync(
                SolutionId,
                model) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(2);
            validationResult["minimum-memory-requirement"].Should().Be("required");
            validationResult["storage-requirements-description"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
