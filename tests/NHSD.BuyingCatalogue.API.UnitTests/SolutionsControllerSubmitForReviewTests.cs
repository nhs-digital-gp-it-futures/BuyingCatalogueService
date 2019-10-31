using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
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
            SetupMockMediator(new SubmitSolutionForReviewCommandResult());

            var result = await _solutionsController.SubmitForReviewAsync(SolutionId) as NoContentResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Test]
        public async Task SubmitForReviewResultFailure()
        {
            var expectedErrorList = new List<ValidationError>{ SubmitSolutionForReviewErrors.SolutionSummaryIsRequired };

            var expected = SubmitSolutionForReviewResult.Create(new ReadOnlyCollection<ValidationError>(expectedErrorList));
            SetupMockMediator(new SubmitSolutionForReviewCommandResult(expectedErrorList));

            var result = await _solutionsController.SubmitForReviewAsync(SolutionId) as BadRequestObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);

            var actual = result.Value as SubmitSolutionForReviewResult;
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }

        private void SetupMockMediator(SubmitSolutionForReviewCommandResult result)
        {
            _mockMediator.Setup(m =>
                m.Send(It.Is<SubmitSolutionForReviewCommand>(q => q.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);
        }
    }
}
