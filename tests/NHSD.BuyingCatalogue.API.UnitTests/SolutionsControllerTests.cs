using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private SolutionsController _solutionsController;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldListSolutions()
        {
            var expected = new ListSolutionsResult(new SolutionSummaryViewModel[0]);

            _mockMediator.Setup(m => m.Send(It.IsAny<ListSolutionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = (await _solutionsController.ListAsync()).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            
            result.Value.Should().Be(expected);
        }

        [Test]
        public async Task ShouldListSolutionsByFilter()
        {
            var filter = new ListSolutionsFilter() { Capabilities = { Guid.Empty } };
            var expected = new ListSolutionsResult(new SolutionSummaryViewModel[0]);

            _mockMediator.Setup(m => m.Send(It.IsAny<ListSolutionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = (await _solutionsController.ListByFilterAsync(filter)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().Be(expected);
        }
    }
}
