using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
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
        public async Task ShouldGetFeatures()
        {
            var expected = new List<string>(){"a", "b", "c"};
            var solution = new Solution {Features = expected};
          
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _featuresController.GetFeaturesAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as FeaturesResult).Listing.Should().BeEquivalentTo(expected);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
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
