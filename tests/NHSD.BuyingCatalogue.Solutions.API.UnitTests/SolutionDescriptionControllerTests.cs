using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionDescriptionControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private SolutionDescriptionController solutionDescriptionController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            solutionDescriptionController = new SolutionDescriptionController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetSolutionDescription()
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Summary == "summary"
                && s.Description == "desc"
                && s.AboutUrl == "test";

            var solutionMock = Mock.Of(solution);
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(solutionMock);

            var result = await solutionDescriptionController.GetSolutionDescriptionAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            ((SolutionDescriptionResult)result.Value).Summary.Should().Be(solutionMock.Summary);
            ((SolutionDescriptionResult)result.Value).Description.Should().Be(solutionMock.Description);
            ((SolutionDescriptionResult)result.Value).Link.Should().Be(solutionMock.AboutUrl);

            mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel { Summary = "Summary" };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            mockMediator.Setup(m => m.Send(
                It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await solutionDescriptionController.UpdateAsync(
                SolutionId,
                solutionSummaryUpdateViewModel) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var solutionSummaryUpdateViewModel = new UpdateSolutionSummaryViewModel
            {
                Summary = string.Empty,
            };

            var validationErrors = new Dictionary<string, string>
            {
                { "description", "maxLength" },
                { "link", "maxLength" },
                { "summary", "required" },
            };

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(validationErrors);
            validationModel.Setup(s => s.IsValid).Returns(false);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await solutionDescriptionController.UpdateAsync(
                SolutionId,
                solutionSummaryUpdateViewModel) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(3);
            resultValue["summary"].Should().Be("required");
            resultValue["description"].Should().Be("maxLength");
            resultValue["link"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionSummaryCommand>(q => q.SolutionId == SolutionId && q.Data == solutionSummaryUpdateViewModel),
                It.IsAny<CancellationToken>()));
        }
    }
}
