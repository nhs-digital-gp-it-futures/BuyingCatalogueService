using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Contracts.SolutionList;
using NHSD.BuyingCatalogue.SolutionLists.API.ViewModels;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.SolutionLists.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionListControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private SolutionListController _solutionListController;

        private IEnumerable<ISolutionSummary> _solutions;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionListController = new SolutionListController(_mockMediator.Object);

            _solutions = new[]
            {
                Mock.Of<ISolutionSummary>(s => s.Id == "Sln1"), Mock.Of<ISolutionSummary>(s => s.Id == "Sln2")
            };
            var solutionList = Mock.Of<ISolutionList>(s => s.Solutions == _solutions);
            _mockMediator.Setup(m => m.Send(It.IsAny<ListSolutionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solutionList);
        }

        [Test]
        public async Task ShouldListFoundationSolutions()
        {
            var result = (await _solutionListController.ListFoundationAsync()).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListSolutionsResult).Solutions.Should().BeEquivalentTo(_solutions);
            _mockMediator.Verify(
                m => m.Send(It.Is<ListSolutionsQuery>(q => q.IsFoundation), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldListSolutions()
        {
            var result = (await _solutionListController.ListAsync()).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListSolutionsResult).Solutions.Should().BeEquivalentTo(_solutions);
            _mockMediator.Verify(
                m => m.Send(It.Is<ListSolutionsQuery>(q => q.IsFoundation == false), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldListSolutionsByFilter()
        {
            var filter = new ListSolutionsFilter {Capabilities = {Guid.Empty}};

            var result = (await _solutionListController.ListByFilterAsync(filter)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListSolutionsResult).Solutions.Should().BeEquivalentTo(_solutions);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<ListSolutionsQuery>(q =>
                        q.CapabilityIdList == filter.Capabilities && q.IsFoundation == false),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void NullMediatorShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionListController(null));
        }
    }
}
