using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;

    namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
    {
        [TestFixture]
        public sealed class NativeDesktopThirdPartyControllerTests
        {
            private Mock<IMediator> _mediatorMock;
            private NativeDesktopMemoryAndStorageController _nativeDesktopMemoryAndStorageController;
            private readonly string _solutionId = "Sln1";
            private Mock<ISimpleResult> _simpleResultMock;
            private Dictionary<string, string> _resultDictionary;

            [SetUp]
            public void Setup()
            {
                _mediatorMock = new Mock<IMediator>();
                _nativeDesktopMemoryAndStorageController = new NativeDesktopMemoryAndStorageController(_mediatorMock.Object);
                _simpleResultMock = new Mock<ISimpleResult>();
                _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
                _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
                _resultDictionary = new Dictionary<string, string>();
                _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<UpdateNativeDesktopMemoryAndStorageCommand>(),
                            It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => _simpleResultMock.Object);
            }

            [Test]
            public async Task UpdateValidMemoryAndStorageDetails()
            {
                var request = new UpdateNativeDesktopMemoryAndStorageViewModel
                {
                    MinimumMemoryRequirement = "1 bit",
                    StorageRequirementsDescription = "A description",
                    MinimumCpu = "1Hz",
                    RecommendedResolution = "1000,00000,0000x1"
                };

                var result =
                    (await _nativeDesktopMemoryAndStorageController.Update(_solutionId, request)
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
                    RecommendedResolution = "1000,00000,0000x1"
                };

                var result =
                    (await _nativeDesktopMemoryAndStorageController.Update(_solutionId, request)
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

