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
    public sealed class FeaturesControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private FeaturesController _featuresController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _featuresController = new FeaturesController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var featuresUpdateViewModel = new UpdateSolutionFeaturesViewModel();
            var result =
                (await _featuresController.UpdateFeaturesAsync(SolutionId, featuresUpdateViewModel)) as
                    NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionFeaturesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionFeaturesViewModel == featuresUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
