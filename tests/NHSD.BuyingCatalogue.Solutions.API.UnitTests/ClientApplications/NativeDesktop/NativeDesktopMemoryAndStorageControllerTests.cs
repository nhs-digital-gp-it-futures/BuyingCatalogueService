using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
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
    namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
    {
        [TestFixture]
        public sealed class NativeDesktopThirdPartyControllerTests
        {
            private Mock<IMediator> _mediatorMock;
            private NativeDesktopMemoryAndStorageController _controller;
            private readonly string _solutionId = "Sln1";
            private Mock<ISimpleResult> _simpleResultMock;
            private Dictionary<string, string> _resultDictionary;

            [SetUp]
            public void Setup()
            {
                _mediatorMock = new Mock<IMediator>();
                _controller = new NativeDesktopMemoryAndStorageController(_mediatorMock.Object);
                _simpleResultMock = new Mock<ISimpleResult>();
                _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
                _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
                _resultDictionary = new Dictionary<string, string>();
                _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<UpdateNativeDesktopMemoryAndStorageCommand>(),
                            It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => _simpleResultMock.Object);
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
                _mediatorMock.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == _solutionId),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                        c.NativeDesktopMemoryAndStorage == Mock.Of<INativeDesktopMemoryAndStorage>(t =>
                            t.MinimumMemoryRequirement == memory &&
                            t.StorageRequirementsDescription == storage &&
                            t.MinimumCpu == minimumCpu &&
                            t.RecommendedResolution == resolution)));

                var result = await _controller.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
                result.Should().NotBeNull();
                result.StatusCode.Should().Be((int)HttpStatusCode.OK);
                result.Value.Should().BeOfType<GetNativeDesktopMemoryAndStorageResult>();
                var memoryAndStorageResult = result.Value as GetNativeDesktopMemoryAndStorageResult;
                memoryAndStorageResult.MinimumMemoryRequirement.Should().Be(memory);
                memoryAndStorageResult.StorageRequirementsDescription.Should().Be(storage);
                memoryAndStorageResult.MinimumCpu.Should().Be(minimumCpu);
                memoryAndStorageResult.RecommendedResolution.Should().Be(resolution);
            }

            [Test]
            public async Task NullClientApplicationShouldReturnNull()
            {
                _mediatorMock.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == _solutionId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(null as IClientApplication);

                var result = (await _controller.Get(_solutionId).ConfigureAwait(false)) as ObjectResult;

                result.StatusCode.Should().Be((int)HttpStatusCode.OK);
                result.Value.Should().BeOfType<GetNativeDesktopMemoryAndStorageResult>();
                var memoryAndStorageResult = result.Value as GetNativeDesktopMemoryAndStorageResult;
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

                var result =
                    (await _controller.Update(_solutionId, request)
                        .ConfigureAwait(false)) as NoContentResult;
                result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
                _mediatorMock.Verify(
                    x => x.Send(
                        It.Is<UpdateNativeDesktopMemoryAndStorageCommand>(c =>
                            c.SolutionId == _solutionId &&
                            c.Data.MinimumMemoryRequirement == request.MinimumMemoryRequirement &&
                            c.Data.StorageRequirementsDescription == request.StorageRequirementsDescription &&
                            c.Data.MinimumCpu == request.MinimumCpu &&
                            c.Data.RecommendedResolution == request.RecommendedResolution
                        ),
                        It.IsAny<CancellationToken>()), Times.Once);
            }

            [Test]
            public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
            {
                _resultDictionary.Add("minimum-memory-requirement", "required");
                _resultDictionary.Add("storage-requirements-description", "maxLength");

                var request = new UpdateNativeDesktopMemoryAndStorageViewModel
                {
                    MinimumMemoryRequirement = "1 bit",
                    StorageRequirementsDescription = "A description",
                    MinimumCpu = "1Hz",
                    RecommendedResolution = "1000,00000,0000x1",
                };

                var result =
                    (await _controller.Update(_solutionId, request)
                        .ConfigureAwait(false)) as BadRequestObjectResult;

                result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                var validationResult = result.Value as Dictionary<string, string>;
                validationResult.Count.Should().Be(2);
                validationResult["minimum-memory-requirement"].Should().Be("required");
                validationResult["storage-requirements-description"].Should().Be("maxLength");

                _mediatorMock.Verify(
                    x => x.Send(
                        It.Is<UpdateNativeDesktopMemoryAndStorageCommand>(c =>
                            c.SolutionId == _solutionId &&
                            c.Data.MinimumMemoryRequirement == request.MinimumMemoryRequirement &&
                            c.Data.StorageRequirementsDescription == request.StorageRequirementsDescription &&
                            c.Data.MinimumCpu == request.MinimumCpu &&
                            c.Data.RecommendedResolution == request.RecommendedResolution
                        ),
                        It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}

