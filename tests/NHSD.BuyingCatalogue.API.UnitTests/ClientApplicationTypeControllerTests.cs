using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Solution.API.Controllers;
using NHSD.BuyingCatalogue.Solution.API.ViewModels;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class ClientApplicationTypeControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private ClientApplicationTypeController _clientApplicationTypeController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _clientApplicationTypeController = new ClientApplicationTypeController(_mockMediator.Object);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetClientApplicationTypes(bool hasClientApplicationTypes)
        {
            var applicationTypes = hasClientApplicationTypes
                ? new HashSet<string> {"browser-based", "native-desktop"}
                : null;

            _mockMediator.Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == applicationTypes));

            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetClientApplicationTypesResult).ClientApplicationTypes.Should().BeEquivalentTo(hasClientApplicationTypes ? (new HashSet<string> { "browser-based", "native-desktop" }) : new HashSet<string>());

            _mockMediator.Verify(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetEmptyClientApplicationTypes()
        {
            _mockMediator.Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>());

            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetClientApplicationTypesResult).ClientApplicationTypes.Should().BeEmpty();

            _mockMediator.Verify(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var clientApplicationUpdateViewModel = new UpdateSolutionClientApplicationTypesViewModel();

            var validationModel = new UpdateSolutionClientApplicationTypesValidationResult();

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result =
                (await _clientApplicationTypeController.UpdateClientApplicationTypesAsync(SolutionId,
                    clientApplicationUpdateViewModel)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var clientApplicationUpdateViewModel = new UpdateSolutionClientApplicationTypesViewModel();

            var validationModel = new UpdateSolutionClientApplicationTypesValidationResult();
            validationModel.Required.Add("client-application-types");

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result =
                (await _clientApplicationTypeController.UpdateClientApplicationTypesAsync(SolutionId,
                    clientApplicationUpdateViewModel)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateSolutionClientApplicationTypesResult).Required.Should().BeEquivalentTo(new [] { "client-application-types" });

            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
