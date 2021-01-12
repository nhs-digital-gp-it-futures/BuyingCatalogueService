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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionDescriptionControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private SolutionDescriptionController _solutionDescriptionController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionDescriptionController = new SolutionDescriptionController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetSolutionDescription()
        {
            var solutionMock = Mock.Of<ISolution>(s => s.Summary == "summary" && s.Description == "desc" && s.AboutUrl == "test");
            _mockMediator.Setup(m =>
                        m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(solutionMock);

            var result = (await _solutionDescriptionController.GetSolutionDescriptionAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            ((SolutionDescriptionResult)result.Value).Summary.Should().Be(solutionMock.Summary);
            ((SolutionDescriptionResult)result.Value).Description.Should().Be(solutionMock.Description);
            ((SolutionDescriptionResult)result.Value).Link.Should().Be(solutionMock.AboutUrl);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel { Summary = "Summary" };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result =
                (await _solutionDescriptionController.UpdateAsync(SolutionId, solutionSummaryUpdateViewModel).ConfigureAwait(false)) as
                    NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel()
            {
                Summary = string.Empty,
            };

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "description", "maxLength" }, { "link", "maxLength" }, { "summary", "required" } });
            validationModel.Setup(s => s.IsValid).Returns(false);

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result =
                (await _solutionDescriptionController.UpdateAsync(SolutionId, solutionSummaryUpdateViewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(3);
            resultValue["summary"].Should().Be("required");
            resultValue["description"].Should().Be("maxLength");
            resultValue["link"].Should().Be("maxLength");

            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
