using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
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

            var result = (await _solutionDescriptionController.GetSolutionDescriptionAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            ((SolutionDescriptionResult) result.Value).Summary.Should().Be(solutionMock.Summary);
            ((SolutionDescriptionResult) result.Value).Description.Should().Be(solutionMock.Description);
            ((SolutionDescriptionResult) result.Value).Link.Should().Be(solutionMock.AboutUrl);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel { Summary = "Summary" };
            var validationModel = new UpdateSolutionSummaryValidationResult();

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionSummaryViewModel == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result =
                (await _solutionDescriptionController.UpdateAsync(SolutionId, solutionSummaryUpdateViewModel)) as
                    NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionSummaryViewModel == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel()
            {
                Summary = string.Empty
            };

            var validationModel = new UpdateSolutionSummaryValidationResult()
            {
                Required = { "summary" },
                MaxLength = { "summary", "description", "link" }
            };

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionSummaryViewModel == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);

            var result =
                (await _solutionDescriptionController.UpdateAsync(SolutionId, solutionSummaryUpdateViewModel)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as UpdateSolutionSummaryResult;
            resultValue.Required.Should().BeEquivalentTo(new[] { "summary" });
            resultValue.MaxLength.Should().BeEquivalentTo(new[] { "summary", "description", "link" });

            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionSummaryViewModel == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
