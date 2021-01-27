using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.HybridHostingType;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.HostingTypes;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hosting;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Hosting
{
    [TestFixture]
    internal sealed class UpdateHybridHostingTypeTests : HostingTestsBase
    {
        private const string SolutionId = "Sln1";
        private const string SummaryToken = "HybridHostingType.Summary";
        private const string LinkToken = "HybridHostingType.Link";
        private const string HostingModelToken = "HybridHostingType.HostingModel";
        private const string RequiresHscnToken = "HybridHostingType.RequiresHSCN";

        private Mock<IUpdateHybridHostingTypeData> dataMock;
        private string updatedSummary;
        private string updatedLink;
        private string updatedHostingModel;
        private string updatedRequiresHscn;

        private Application.Domain.Hosting initialHosting;
        private string initialHostingJson;

        [SetUp]
        public void Setup()
        {
            initialHosting = new Application.Domain.Hosting
            {
                HybridHostingType = new HybridHostingType
                {
                    Summary = "A summary",
                    Link = "A link",
                    HostingModel = "A _initialHosting model",
                    RequiresHSCN = "A string",
                },
            };

            initialHostingJson = JsonConvert.SerializeObject(initialHosting);

            updatedSummary = "A summary";
            updatedLink = "A link";
            updatedHostingModel = "A _initialHosting model";
            updatedRequiresHscn = "A requires string";

            dataMock = new Mock<IUpdateHybridHostingTypeData>();
            dataMock.Setup(d => d.Summary).Returns(() => updatedSummary);
            dataMock.Setup(d => d.Link).Returns(() => updatedLink);
            dataMock.Setup(d => d.HostingModel).Returns(() => updatedHostingModel);
            dataMock.Setup(d => d.RequiresHSCN).Returns(() => updatedRequiresHscn);
        }

        [Test]
        public async Task ValidDataShouldUpdateHybridHostingType()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(initialHostingJson);
            var validationResult = await Update();

            Expression<Func<IUpdateSolutionHostingRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.Hosting).SelectToken(SummaryToken).Value<string>() == updatedSummary
                && JToken.Parse(r.Hosting).SelectToken(LinkToken).Value<string>() == updatedLink
                && JToken.Parse(r.Hosting).SelectToken(HostingModelToken).Value<string>() == updatedHostingModel
                && JToken.Parse(r.Hosting).SelectToken(RequiresHscnToken).Value<string>() == updatedRequiresHscn;

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));
            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.Is(match), It.IsAny<CancellationToken>()));

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task NullDataShouldUpdateHybridHostingType()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(initialHostingJson);

            updatedSummary = null;
            updatedLink = null;
            updatedHostingModel = null;
            updatedRequiresHscn = null;

            var validationResult = await Update();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionHostingRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.Hosting).SelectToken("Hosting") == null;

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.Is(match), It.IsAny<CancellationToken>()));

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateHybridHostingTypeAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(initialHostingJson);

            var calledBack = false;

            void Action(IUpdateSolutionHostingRequest updateHostingRequest, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(updateHostingRequest.Hosting);
                var newHosting = JsonConvert.DeserializeObject<Application.Domain.Hosting>(json.ToString());
                newHosting.Should().BeEquivalentTo(initialHosting, o => o.Excluding(m => m.HybridHostingType));

                newHosting.HybridHostingType.Summary.Should().Be(updatedSummary);
                newHosting.HybridHostingType.Link.Should().Be(updatedLink);
                newHosting.HybridHostingType.HostingModel.Should().Be(updatedHostingModel);
                newHosting.HybridHostingType.RequiresHSCN.Should().BeEquivalentTo(updatedRequiresHscn);
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateHostingAsync(
                    It.IsAny<IUpdateSolutionHostingRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionHostingRequest, CancellationToken>(Action);

            var validationResult = await Update();
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [TestCase(500, 1000, 1001, "hosting-model")]
        [TestCase(500, 1001, 1000, "link")]
        [TestCase(501, 1000, 1000, "summary")]
        [TestCase(501, 1001, 1001, "summary", "link", "hosting-model")]
        public async Task TooLongDataShouldReturnRequiredValidationResult(
            int summary,
            int link,
            int hosting,
            params string[] expected)
        {
            updatedSummary = new string('a', summary);
            updatedLink = new string('a', link);
            updatedHostingModel = new string('a', hosting);

            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await Update();

            Context.MockSolutionRepository.Verify(
                r => r.ByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never());

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.IsAny<IUpdateSolutionHostingRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<MaxLengthResult>();

            var maxLengthResult = validationResult as MaxLengthResult;

            Assert.NotNull(maxLengthResult);
            maxLengthResult.MaxLength.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void NoSolutionReturnedShouldThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await Update());

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));
            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.IsAny<IUpdateSolutionHostingRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        private async Task<ISimpleResult> Update()
        {
            return await Context.UpdateHybridHostingTypeHandler.Handle(
                new UpdateHybridHostingTypeCommand(SolutionId, dataMock.Object),
                CancellationToken.None);
        }
    }
}
