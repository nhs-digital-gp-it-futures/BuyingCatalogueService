using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileMemoryAndStorage;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class MemoryAndStorageControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private MemoryAndStorageController _memoryAndStorageController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _memoryAndStorageController = new MemoryAndStorageController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetMemoryAndStorage()
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MobileMemoryAndStorage == Mock.Of<IMobileMemoryAndStorage>(m =>
                        m.MinimumMemoryRequirement == "1GB" &&
                        m.Description == "desc")));

            var result =
                (await _memoryAndStorageController.GetMemoryAndStorageAsync(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var memoryAndStorage = result?.Value as GetSolutionMemoryAndStorageResult;
            memoryAndStorage?.MinimumMemoryRequirement.Should().Be("1GB");
            memoryAndStorage?.Description.Should().Be("desc");

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetMemoryAndStorageIsNull()
        {
            var clientMock = new Mock<IClientApplication>();
            clientMock.Setup(c => c.MobileOperatingSystems).Returns<IMobileMemoryAndStorage>(null);

            _mockMediator
                .Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(clientMock.Object);

            var result =
                (await _memoryAndStorageController.GetMemoryAndStorageAsync(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var memoryAndStorage = (result?.Value as GetSolutionMemoryAndStorageResult);
            memoryAndStorage?.MinimumMemoryRequirement.Should().BeNull();
            memoryAndStorage?.Description.Should().BeNull();
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result =
                (await _memoryAndStorageController.GetMemoryAndStorageAsync(SolutionId)
                    .ConfigureAwait(false)) as NotFoundResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var viewModel = new UpdateSolutionMemoryAndStorageRequest();

            var validationModel = new RequiredMaxLengthResult();

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionMobileMemoryStorageCommand>(q =>
                        q.Id == SolutionId && q.MinimumMemoryRequirement == viewModel.MinimumMemoryRequirement &&
                        q.Description == viewModel.Description), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel);

            var result =
                (await _memoryAndStorageController.UpdateMemoryAndStorageAsync(SolutionId, viewModel)
                    .ConfigureAwait(false)) as NoContentResult;

            result?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileMemoryStorageCommand>(q =>
                        q.Id == SolutionId && q.MinimumMemoryRequirement == viewModel.MinimumMemoryRequirement &&
                        q.Description == viewModel.Description), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var viewModel = new UpdateSolutionMemoryAndStorageRequest();

            var validationModel = new RequiredMaxLengthResult()
            {
                Required = { "minimum-memory-requirement", "storage-requirements-description" },
                MaxLength = { "storage-requirements-description" }
            };

            _mockMediator.Setup(m =>
                m.Send(
                    It.Is<UpdateSolutionMobileMemoryStorageCommand>(q =>
                        q.Id == SolutionId && q.MinimumMemoryRequirement == viewModel.MinimumMemoryRequirement &&
                        q.Description == viewModel.Description),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result = (await _memoryAndStorageController.UpdateMemoryAndStorageAsync(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateFormRequiredMaxLengthResult).Required.Should().BeEquivalentTo(new[] { "minimum-memory-requirement", "storage-requirements-description" });
            (result.Value as UpdateFormRequiredMaxLengthResult).MaxLength.Should().BeEquivalentTo(new[] { "storage-requirements-description" });

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionMobileMemoryStorageCommand>(q =>
                        q.Id == SolutionId && q.MinimumMemoryRequirement ==
                        viewModel.MinimumMemoryRequirement && q.Description == viewModel.Description),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
