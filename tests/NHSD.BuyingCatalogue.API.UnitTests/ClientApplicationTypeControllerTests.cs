using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
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

        [Test]
        public async Task ShouldGetClientApplicationTypes()
        {
            _mockMediator.Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication == Mock.Of<IClientApplication>(c =>
                        c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-desktop" })));

            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetClientApplicationTypesResult).ClientApplicationTypes.Should().BeEquivalentTo(new string[] { "browser-based", "native-desktop" });

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetEmptyClientApplicationTypes()
        {
            _mockMediator.Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISolution>());

            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetClientApplicationTypesResult).ClientApplicationTypes.Should().BeEmpty();

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var clientApplicationUpdateViewModel = new UpdateSolutionClientApplicationTypesViewModel();
            var result =
                (await _clientApplicationTypeController.UpdateClientApplicationTypesAsync(SolutionId,
                    clientApplicationUpdateViewModel)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionClientApplicationTypesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionClientApplicationTypesViewModel == clientApplicationUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
