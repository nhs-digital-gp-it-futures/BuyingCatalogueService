using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerSubmitForReviewTests
    {
        private Mock<IMediator> _mockMediator;

        private SolutionsController _solutionsController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }


        [Test]
        public async Task SubmitForReviewResultSuccess()
        {
            SetupMockMediator(SubmitSolutionForReviewResult.Success);

            var result = _solutionsController.SubmitForReviewAsync(SolutionId).Result as NoContentResult;
            result.StatusCode.Should().Be(204);
        }

        [Test]
        public async Task SubmitForReviewResultFailure()
        {
            var expected = SubmitSolutionForReviewResult.Failure;
            SetupMockMediator(expected);

            var result = _solutionsController.SubmitForReviewAsync(SolutionId).Result as BadRequestObjectResult;
            result.StatusCode.Should().Be(400);
            (result.Value as SubmitSolutionForReviewResult).Should().Be(expected);
        }

        private void SetupMockMediator(SubmitSolutionForReviewResult successResult)
        {
            _mockMediator.Setup(m =>
                m.Send(It.Is<SubmitSolutionForReviewCommand>(q => q.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(successResult);
        }
    }
}
