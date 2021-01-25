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
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class ClientApplicationTypeControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private ClientApplicationTypeController clientApplicationTypeController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            clientApplicationTypeController = new ClientApplicationTypeController(mockMediator.Object);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetClientApplicationTypes(bool hasClientApplicationTypes)
        {
            var applicationTypes = hasClientApplicationTypes
                ? new HashSet<string> { "browser-based", "native-desktop" }
                : null;

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.ClientApplicationTypes == applicationTypes));

            var result = await clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<GetClientApplicationTypesResult>();
            result.Value.As<GetClientApplicationTypesResult>().ClientApplicationTypes.Should().BeEquivalentTo(
                hasClientApplicationTypes ? new HashSet<string> { "browser-based", "native-desktop" } : new HashSet<string>());

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetEmptyClientApplicationTypes()
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>());

            var result = await clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<GetClientApplicationTypesResult>();
            result.Value.As<GetClientApplicationTypesResult>().ClientApplicationTypes.Should().BeEmpty();

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = await clientApplicationTypeController.GetClientApplicationTypesAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<GetClientApplicationTypesResult>();
            result.Value.As<GetClientApplicationTypesResult>().ClientApplicationTypes.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var clientApplicationUpdateModel = new UpdateSolutionClientApplicationTypesViewModel(null);

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            Expression<Func<UpdateSolutionClientApplicationTypesCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.Data == clientApplicationUpdateModel;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await clientApplicationTypeController.UpdateClientApplicationTypesAsync(
                SolutionId,
                clientApplicationUpdateModel) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var clientApplicationUpdateModel = new UpdateSolutionClientApplicationTypesViewModel(null);

            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string>
            {
                { "client-application-types", "required" },
            });

            validationModel.Setup(s => s.IsValid).Returns(false);

            Expression<Func<UpdateSolutionClientApplicationTypesCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.Data == clientApplicationUpdateModel;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await clientApplicationTypeController.UpdateClientApplicationTypesAsync(
                SolutionId,
                clientApplicationUpdateModel) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["client-application-types"].Should().Be("required");

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
