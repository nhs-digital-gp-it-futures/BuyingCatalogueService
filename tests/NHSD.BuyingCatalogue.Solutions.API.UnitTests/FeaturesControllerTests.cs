using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
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
            var expected = new List<string>() { "a", "b", "c" };

            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISolution>(s => s.Features == expected));

            var result = (await _featuresController.GetFeaturesAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as FeaturesResult).Listing.Should().BeEquivalentTo(expected);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var featuresUpdateViewModel = new UpdateSolutionFeaturesViewModel()
            {
                Listing = new List<string>() { new string('a', 200) }
            };

            var validationModel = new UpdateSolutionFeaturesValidationResult()
            {
                MaxLength = { "listing-1" }
            };
            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionFeaturesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionFeaturesViewModel == featuresUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);
            var result =
                (await _featuresController.UpdateFeaturesAsync(SolutionId, featuresUpdateViewModel)) as
                    BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateSolutionFeaturesResult).MaxLength.Should().BeEquivalentTo("listing-1"); 
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionFeaturesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionFeaturesViewModel == featuresUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var featuresUpdateViewModel = new UpdateSolutionFeaturesViewModel()
            {
                Listing = new List<string>() { "test", "test2" }
            };

            var validationModel = new UpdateSolutionFeaturesValidationResult();

            _mockMediator.Setup(m => m.Send(It.Is<UpdateSolutionFeaturesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionFeaturesViewModel == featuresUpdateViewModel), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel);
            var result =
                (await _featuresController.UpdateFeaturesAsync(SolutionId, featuresUpdateViewModel)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionFeaturesCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionFeaturesViewModel == featuresUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void NullSolutionShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FeaturesResult(null));
        }

    }
}
