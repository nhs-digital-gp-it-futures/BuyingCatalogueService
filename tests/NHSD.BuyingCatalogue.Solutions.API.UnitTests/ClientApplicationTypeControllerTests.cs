using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
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
                ? new HashSet<string> { "browser-based", "native-desktop" }
                : null;

            _mockMediator.Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == applicationTypes));

            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetClientApplicationTypesResult).ClientApplicationTypes.Should().BeEquivalentTo(hasClientApplicationTypes ? (new HashSet<string> { "browser-based", "native-desktop" }) : new HashSet<string>());

            _mockMediator.Verify(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetEmptyClientApplicationTypes()
        {
            _mockMediator.Setup(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>());

            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetClientApplicationTypesResult).ClientApplicationTypes.Should().BeEmpty();

            _mockMediator.Verify(m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetClientApplicationTypesResult).ClientApplicationTypes.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var clientApplicationUpdateViewModel = new UpdateSolutionClientApplicationTypesViewModel();

            var validationModel = new RequiredResult();

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result =
                (await _clientApplicationTypeController.UpdateClientApplicationTypesAsync(SolutionId,
                    clientApplicationUpdateViewModel).ConfigureAwait(false)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var clientApplicationUpdateViewModel = new UpdateSolutionClientApplicationTypesViewModel();

            var validationModel = new RequiredResult();
            validationModel.Required.Add("client-application-types");

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result =
                (await _clientApplicationTypeController.UpdateClientApplicationTypesAsync(SolutionId,
                    clientApplicationUpdateViewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["client-application-types"].Should().Be("required");

            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
