using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.SolutionLists.API.ViewModels;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.SolutionLists.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionListControllerTests
    {
        private Mock<IMediator> mockMediator;
        private SolutionListController solutionListController;
        private IEnumerable<ISolutionSummary> solutions;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            solutionListController = new SolutionListController(mockMediator.Object);

            solutions = new[]
            {
                Mock.Of<ISolutionSummary>(s => s.Id == "Sln1"), Mock.Of<ISolutionSummary>(s => s.Id == "Sln2"),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison
            var solutionList = Mock.Of<ISolutionList>(s => s.Solutions == solutions);

            mockMediator
                .Setup(m => m.Send(It.IsAny<ListSolutionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solutionList);
        }

        [Test]
        public async Task ShouldListFoundationSolutions()
        {
            var result = (await solutionListController.ListFoundationAsync()).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<ListSolutionsResult>();
            result.Value.As<ListSolutionsResult>().Solutions.Should().BeEquivalentTo(solutions);

            mockMediator.Verify(m => m.Send(
                It.Is<ListSolutionsQuery>(q => q.Data.IsFoundation),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldListSolutions()
        {
            var result = (await solutionListController.ListAsync(null, null)).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<ListSolutionsResult>();
            result.Value.As<ListSolutionsResult>().Solutions.Should().BeEquivalentTo(solutions);

            mockMediator.Verify(m => m.Send(
                It.Is<ListSolutionsQuery>(q => q.Data.IsFoundation == false),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ListAsync_HasSupplierId_ReturnsSolutionsByFilteredSupplierId()
        {
            const string supplierId = "sup1";

            var result = (await solutionListController.ListAsync(supplierId, null)).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<ListSolutionsResult>();
            result.Value.As<ListSolutionsResult>().Solutions.Should().BeEquivalentTo(solutions);

            mockMediator.Verify(m => m.Send(
                It.Is<ListSolutionsQuery>(q => q.Data.SupplierId == supplierId && q.Data.IsFoundation == false),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldListSolutionsByFilter()
        {
            var filter = new ListSolutionsFilterViewModel();
            var result = (await solutionListController.ListByFilterAsync(filter)).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<ListSolutionsResult>();
            result.Value.As<ListSolutionsResult>().Solutions.Should().BeEquivalentTo(solutions);

            mockMediator.Verify(m => m.Send(
                It.Is<ListSolutionsQuery>(q => q.Data.Equals(filter)),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public void NullMediatorShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new SolutionListController(null));
        }

        [Test]
        public void ListSolutionsResultThrowsIfNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ListSolutionsResult(null));
        }
    }
}
