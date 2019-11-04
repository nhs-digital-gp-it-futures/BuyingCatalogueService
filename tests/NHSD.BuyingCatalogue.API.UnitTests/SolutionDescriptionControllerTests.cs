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
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture, Ignore("Update to handle validation")]
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
           var solution = new Solution { Summary = "summary", Description = "desc", AboutUrl = "test"};

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _solutionDescriptionController.GetSolutionDescriptionAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as SolutionDescriptionResult).Summary.Should().Be(solution.Summary);
            (result.Value as SolutionDescriptionResult).Description.Should().Be(solution.Description);
            (result.Value as SolutionDescriptionResult).Link.Should().Be(solution.AboutUrl);


            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel {Summary = "Summary"};
            var result =
                (await _solutionDescriptionController.UpdateAsync(SolutionId, solutionSummaryUpdateViewModel)) as
                    NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionSummaryViewModel == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
