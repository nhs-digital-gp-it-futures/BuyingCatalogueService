using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.SolutionLists.API.ViewModels;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
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
            var result = (await _solutionListController.ListFoundationAsync().ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListSolutionsResult).Solutions.Should().BeEquivalentTo(_solutions);
            _mockMediator.Verify(
                m => m.Send(It.Is<ListSolutionsQuery>(q => q.Data.IsFoundation), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldListSolutions()
        {
            var result = (await _solutionListController.ListAsync(null).ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListSolutionsResult).Solutions.Should().BeEquivalentTo(_solutions);
            _mockMediator.Verify(
                m => m.Send(It.Is<ListSolutionsQuery>(q => q.Data.IsFoundation == false), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ListAsync_HasSupplierId_ReturnsSolutionsByFilteredSupplierId()
        {
            var supplierId = "sup1";

            var result = (await _solutionListController.ListAsync(supplierId).ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListSolutionsResult).Solutions.Should().BeEquivalentTo(_solutions);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<ListSolutionsQuery>(q => q.Data.SupplierId == supplierId && q.Data.IsFoundation == false),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldListSolutionsByFilter()
        {
            var filter = new ListSolutionsFilterViewModel();
            var result = (await _solutionListController.ListByFilterAsync(filter).ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListSolutionsResult).Solutions.Should().BeEquivalentTo(_solutions);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<ListSolutionsQuery>(q => q.Data.IsSameOrEqualTo(filter)),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void NullMediatorShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionListController(null));
        }

        [Test]
        public void ListSolutionsResultThrowsIfNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ListSolutionsResult(null));
        }
    }
}
