using System;
using System.Collections.Generic;
using System.Linq;
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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopMemoryAndStorageControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private NativeDesktopMemoryAndStorageController controller;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new NativeDesktopMemoryAndStorageController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateNativeDesktopMemoryAndStorageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase("512TB", "1024TB", "1Hz", "1x1 px")]
        [TestCase(null, "1024TB", "1Hz", "1x1 px")]
        [TestCase("512TB", null, "1Hz", "1x1 px")]
        [TestCase("512TB", "1024TB", null, "1x1 px")]
        [TestCase("512TB", "1024TB", "1Hz", null)]
        [TestCase(null, "1024TB", "1Hz", null)]
        [TestCase("512TB", null, null, "1x1 px")]
        [TestCase("512TB", null, null, "1x1 px")]
        [TestCase(null, "1024TB", "1Hz", null)]
        [TestCase(null, "1024TB", null, null)]
        [TestCase(null, null, null, "1x1 px")]
        [TestCase("512TB", null, null, null)]
        [TestCase(null, null, "1Hz", null)]
        [TestCase(null, null, null, null)]
        public async Task PopulatedDataShouldReturnCorrectData(string memory, string storage, string minimumCpu, string resolution)
        {
            Expression<Func<INativeDesktopMemoryAndStorage, bool>> nativeDesktopMemoryAndStorage = d =>
                d.MinimumMemoryRequirement == memory
                && d.StorageRequirementsDescription == storage
                && d.MinimumCpu == minimumCpu
                && d.RecommendedResolution == resolution;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.NativeDesktopMemoryAndStorage == Mock.Of(nativeDesktopMemoryAndStorage);

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of(clientApplication));

            var result = await controller.Get(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetNativeDesktopMemoryAndStorageResult>();

            var memoryAndStorageResult = result.Value as GetNativeDesktopMemoryAndStorageResult;

            Assert.NotNull(memoryAndStorageResult);
            memoryAndStorageResult.MinimumMemoryRequirement.Should().Be(memory);
            memoryAndStorageResult.StorageRequirementsDescription.Should().Be(storage);
            memoryAndStorageResult.MinimumCpu.Should().Be(minimumCpu);
            memoryAndStorageResult.RecommendedResolution.Should().Be(resolution);
        }

        [Test]
        public async Task NullClientApplicationShouldReturnNull()
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IClientApplication);

            var result = await controller.Get(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetNativeDesktopMemoryAndStorageResult>();

            var memoryAndStorageResult = result.Value as GetNativeDesktopMemoryAndStorageResult;

            Assert.NotNull(memoryAndStorageResult);
            memoryAndStorageResult.MinimumMemoryRequirement.Should().BeNull();
            memoryAndStorageResult.StorageRequirementsDescription.Should().BeNull();
            memoryAndStorageResult.MinimumCpu.Should().BeNull();
            memoryAndStorageResult.RecommendedResolution.Should().BeNull();
        }

        [Test]
        public async Task UpdateValidMemoryAndStorageDetails()
        {
            var request = new UpdateNativeDesktopMemoryAndStorageViewModel
            {
                MinimumMemoryRequirement = "1 bit",
                StorageRequirementsDescription = "A description",
                MinimumCpu = "1Hz",
                RecommendedResolution = "1000,00000,0000x1",
            };

            var result = await controller.Update(SolutionId, request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            Expression<Func<UpdateNativeDesktopMemoryAndStorageCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.Data.MinimumMemoryRequirement == request.MinimumMemoryRequirement
                && c.Data.StorageRequirementsDescription == request.StorageRequirementsDescription
                && c.Data.MinimumCpu == request.MinimumCpu
                && c.Data.RecommendedResolution == request.RecommendedResolution;

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("minimum-memory-requirement", "required");
            resultDictionary.Add("storage-requirements-description", "maxLength");

            var request = new UpdateNativeDesktopMemoryAndStorageViewModel
            {
                MinimumMemoryRequirement = "1 bit",
                StorageRequirementsDescription = "A description",
                MinimumCpu = "1Hz",
                RecommendedResolution = "1000,00000,0000x1",
            };

            var result = await controller.Update(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(2);
            validationResult["minimum-memory-requirement"].Should().Be("required");
            validationResult["storage-requirements-description"].Should().Be("maxLength");

            Expression<Func<UpdateNativeDesktopMemoryAndStorageCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.Data.MinimumMemoryRequirement == request.MinimumMemoryRequirement
                && c.Data.StorageRequirementsDescription == request.StorageRequirementsDescription
                && c.Data.MinimumCpu == request.MinimumCpu
                && c.Data.RecommendedResolution == request.RecommendedResolution;

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
