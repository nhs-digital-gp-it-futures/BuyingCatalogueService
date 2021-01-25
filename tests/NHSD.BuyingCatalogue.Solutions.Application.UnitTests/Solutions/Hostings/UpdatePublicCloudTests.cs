using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Hostings
{
    [TestFixture]
    internal sealed class UpdatePublicCloudTests : HostingTestsBase
    {
        private const string SolutionId = "Sln1";
        private const string SummaryToken = "PublicCloud.Summary";
        private const string LinkToken = "PublicCloud.Link";
        private const string RequiresHscnToken = "PublicCloud.RequiresHSCN";

        private Mock<IUpdatePublicCloudData> dataMock;
        private string summary;
        private string url;

        private Hosting hosting;
        private string hostingJson;
        private string requiresHscn;

        [SetUp]
        public void Setup()
        {
            summary = "A Summary";
            url = "A URL";
            requiresHscn = "Requires HSCN";

            dataMock = new Mock<IUpdatePublicCloudData>();
            dataMock.Setup(d => d.Summary).Returns(() => summary);
            dataMock.Setup(d => d.Link).Returns(() => url);
            dataMock.Setup(d => d.RequiresHSCN).Returns(() => requiresHscn);

            hosting = new Hosting
            {
                PublicCloud = new PublicCloud
                {
                    Summary = summary,
                    Link = url,
                    RequiresHSCN = requiresHscn,
                },
            };

            hostingJson = JsonConvert.SerializeObject(hosting);
        }

        [Test]
        public async Task ShouldUpdatePublicCloud()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(hostingJson);
            var validationResult = await UpdatePublicCloud();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionHostingRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.Hosting).SelectToken(SummaryToken).Value<string>() == summary
                && JToken.Parse(r.Hosting).SelectToken(LinkToken).Value<string>() == url
                && JToken.Parse(r.Hosting).SelectToken(RequiresHscnToken).Value<string>() == requiresHscn;

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.Is(match), It.IsAny<CancellationToken>()));

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdatePublicCloudToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(hostingJson);

            summary = null;
            url = null;
            requiresHscn = null;

            var validationResult = await UpdatePublicCloud();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionHostingRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.Hosting).SelectToken("Hosting") == null;

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.Is(match), It.IsAny<CancellationToken>()));

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdatePublicCloudAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(hostingJson);

            var calledBack = false;

            void Action(IUpdateSolutionHostingRequest updateSolutionHostingRequest, CancellationToken _)
            {
                calledBack = true;
                var json = JToken.Parse(updateSolutionHostingRequest.Hosting);
                var newHosting = JsonConvert.DeserializeObject<Hosting>(json.ToString());

                newHosting.PublicCloud.Summary.Should().Be(summary);
                newHosting.PublicCloud.Link.Should().Be(url);
                newHosting.PublicCloud.RequiresHSCN.Should().Be(requiresHscn);
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateHostingAsync(
                    It.IsAny<IUpdateSolutionHostingRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionHostingRequest, CancellationToken>(Action);

            var validationResult = await UpdatePublicCloud();
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [TestCase(501, 1000, "summary")]
        [TestCase(500, 1001, "link")]
        [TestCase(501, 1001, "summary", "link")]
        public async Task ShouldNotUpdateInvalidPublicCloud(int summary, int link, params string[] expected)
        {
            SetUpMockSolutionRepositoryGetByIdAsync(hostingJson);

            this.summary = new string('a', summary);
            url = new string('a', link);

            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdatePublicCloud();

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
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await UpdatePublicCloud());

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.IsAny<IUpdateSolutionHostingRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        private async Task<ISimpleResult> UpdatePublicCloud()
        {
            return await Context.UpdatePublicCloudHandler.Handle(
                new UpdatePublicCloudCommand(SolutionId, dataMock.Object),
                CancellationToken.None);
        }
    }
}
