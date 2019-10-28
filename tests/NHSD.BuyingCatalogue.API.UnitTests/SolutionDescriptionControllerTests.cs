using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
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
        public async Task ShouldUpdate()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel();
            var result =
                (await _solutionDescriptionController.UpdateAsync(SolutionId, solutionSummaryUpdateViewModel)) as
                    NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionSummaryViewModel == solutionSummaryUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
