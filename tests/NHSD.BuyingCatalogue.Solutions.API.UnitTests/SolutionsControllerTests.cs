using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerTests
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
        public void NullMediatorShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionsController(null));
        }

        [Test]
        public async Task GetAsync_Solution_ReturnsExpectedGetSolutionResult()
        {
            var solution = Mock.Of<ISolution>(
                s => s.Id == SolutionId
                && s.Name == "Some solution name"
                && s.Summary == "Some solution summary");

            SetupMockMediator(solution);

            var result = await _solutionsController.GetAsync(SolutionId);

            var expected = new ActionResult<GetSolutionResult>(new GetSolutionResult(null)
            {
                Name = solution.Name,
                Summary = solution.Summary
            });

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAsync_NullSolution_ReturnsExpectedGetSolutionResult()
        {
            SetupMockMediator(null);

            var result = await _solutionsController.GetAsync(SolutionId);

            var expected = new ActionResult<GetSolutionResult>(new GetSolutionResult(null));
            result.Should().BeEquivalentTo(expected);
        }

        private void SetupMockMediator(ISolution result)
        {
            _mockMediator.Setup(m => m.Send(
                    It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(() => result);
        }
    }
}
