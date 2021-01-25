using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class FeaturesControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private FeaturesController featuresController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            featuresController = new FeaturesController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetFeatures()
        {
            var expected = new List<string> { "a", "b", "c" };

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISolution>(s => s.Features == expected));

            var result = await featuresController.GetFeaturesAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<FeaturesResult>();
            result.Value.As<FeaturesResult>().Listing.Should().BeEquivalentTo(expected);

            mockMediator.Verify(m => m.Send(
                It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var featuresUpdateModel = new UpdateSolutionFeaturesViewModel
            {
                Listing = new List<string> { new('a', 200) },
            };

            var validationModel = new Mock<ISimpleResult>();
            validationModel
                .Setup(s => s.ToDictionary())
                .Returns(new Dictionary<string, string> { { "listing-1", "maxLength" } });

            validationModel.Setup(s => s.IsValid).Returns(false);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionFeaturesCommand>(c => c.SolutionId == SolutionId && c.Data == featuresUpdateModel),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await featuresController.UpdateFeaturesAsync(
                SolutionId,
                featuresUpdateModel) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["listing-1"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionFeaturesCommand>(c => c.SolutionId == SolutionId && c.Data == featuresUpdateModel),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var featuresUpdateModel = new UpdateSolutionFeaturesViewModel
            {
                Listing = new List<string> { "test", "test2" },
            };

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionFeaturesCommand>(c => c.SolutionId == SolutionId && c.Data == featuresUpdateModel),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await featuresController.UpdateFeaturesAsync(SolutionId, featuresUpdateModel) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionFeaturesCommand>(c => c.SolutionId == SolutionId && c.Data == featuresUpdateModel),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public void NullSolutionShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new FeaturesResult(null));
        }
    }
}
