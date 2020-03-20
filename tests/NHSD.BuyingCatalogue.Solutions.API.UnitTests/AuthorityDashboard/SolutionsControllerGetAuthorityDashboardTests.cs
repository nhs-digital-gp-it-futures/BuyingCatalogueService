using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.AuthorityDashboard
{
    [TestFixture]
    public sealed class SolutionsControllerGetAuthorityDashboardTests
    {
        private Mock<IMediator> _mockMediator;
        private SolutionsController _solutionsController;
        private const string SolutionId = "Sln1";

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _solutionsController.AuthorityDashboard(SolutionId).ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as SolutionAuthorityDashboardResult).Id.Should().BeNull();
        }
    }
}
