using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionsControllerSubmitForReviewTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private SolutionsController solutionsController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            solutionsController = new SolutionsController(mockMediator.Object);
        }

        [Test]
        public async Task SubmitForReviewResultSuccess()
        {
            SetupMockMediator(new SubmitSolutionForReviewCommandResult());

            var result = await solutionsController.SubmitForReviewAsync(SolutionId) as NoContentResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Test]
        public async Task SubmitForReviewResultFailure()
        {
            var expectedErrorList = new List<ValidationError> { SubmitSolutionForReviewErrors.SolutionSummaryIsRequired };

            var expected = SubmitSolutionForReviewResult.Create(new ReadOnlyCollection<ValidationError>(expectedErrorList));
            SetupMockMediator(new SubmitSolutionForReviewCommandResult(expectedErrorList));

            var result = await solutionsController.SubmitForReviewAsync(SolutionId) as BadRequestObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);

            var actual = result.Value as SubmitSolutionForReviewResult;
            actual.Should().NotBeNull();
            actual.RequiredSections.Should().BeEquivalentTo(expected.RequiredSections);
        }

        [Test]
        public void NullErrorsShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SubmitSolutionForReviewResult.Create(null));
        }

        private void SetupMockMediator(SubmitSolutionForReviewCommandResult result)
        {
            Expression<Func<IMediator, Task<SubmitSolutionForReviewCommandResult>>> expression = m => m.Send(
                It.Is<SubmitSolutionForReviewCommand>(q => q.SolutionId == SolutionId),
                It.IsAny<CancellationToken>());

            mockMediator.Setup(expression).ReturnsAsync(result);
        }
    }
}
