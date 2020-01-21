using System.Collections.Generic;
using System.Linq;
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
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Hostings
{
    [TestFixture]
    internal sealed class UpdatePublicCloudTests : HostingTestsBase
    {
        private const string SolutionId = "Sln1";
        private const string SummaryToken = "PublicCloud.Summary";
        private const string LinkToken = "PublicCloud.Link";
        private const string ConnectivityToken = "PublicCloud.RequiresHSCN";

        private Mock<IUpdatePublicCloudData> _dataMock;
        private string _summary;
        private string _url;
        private HashSet<string> _connectivity;

        private Hosting _hosting;
        private string _hostingJson;

        [SetUp]
        public void Setup()
        {
            _summary = "A Summary";
            _url = "A URL";
            _connectivity = new HashSet<string>{"Some Connectivity"};

            _dataMock = new Mock<IUpdatePublicCloudData>();
            _dataMock.Setup(x => x.Summary).Returns(() => _summary);
            _dataMock.Setup(x => x.Link).Returns(() => _url);
            _dataMock.Setup(x => x.RequiresHSCN).Returns(() => _connectivity);

            _hosting = new Hosting
            {
                PublicCloud = new PublicCloud()
                {
                    Summary = _summary,
                    Link = _url,
                    RequiresHSCN = _connectivity.FirstOrDefault()
                }
            };

            _hostingJson = JsonConvert.SerializeObject(_hosting);
        }

        [Test]
        public async Task ShouldUpdatePublicCloud()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(_hostingJson);
            var validationResult = await UpdatePublicCloud().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateHostingAsync(It.Is<IUpdateSolutionHostingRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.Hosting).SelectToken(SummaryToken).Value<string>() == _summary
                && JToken.Parse(r.Hosting).SelectToken(LinkToken).Value<string>() == _url
                && JToken.Parse(r.Hosting).SelectToken(ConnectivityToken).Value<string>() == _connectivity.FirstOrDefault()
            ), It.IsAny<CancellationToken>()), Times.Once());

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdatePublicCloudToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(_hostingJson);

            _summary = null;
            _url = null;
            _connectivity.Clear();

            var validationResult = await UpdatePublicCloud().ConfigureAwait(false);
         
            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateHostingAsync(It.Is<IUpdateSolutionHostingRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.Hosting).SelectToken("Hosting") == null
            ), It.IsAny<CancellationToken>()), Times.Once());

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdatePublicCloudyAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(_hostingJson);

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateHostingAsync(It.IsAny<IUpdateSolutionHostingRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionHostingRequest updateSolutionHostingRequest,
                    CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionHostingRequest.Hosting);
                    var newHosting = JsonConvert.DeserializeObject<Hosting>(json.ToString());

                    newHosting.PublicCloud.Summary.Should().Be(_summary);
                    newHosting.PublicCloud.Link.Should().Be(_url);
                    newHosting.PublicCloud.RequiresHSCN.Should().Be(_connectivity.FirstOrDefault());

                });
            var validationResult = await UpdatePublicCloud().ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [TestCase(501, 1000, "summary")]
        [TestCase(500, 1001, "link")]
        [TestCase(501, 1001, "summary", "link")]
        public async Task ShouldNotUpdateInvalidPublicCloud(int summary, int link, params string[] expected)
        {
            SetUpMockSolutionRepositoryGetByIdAsync(_hostingJson);

            _summary = new string('a', summary);
            _url = new string('a', link);

            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await UpdatePublicCloud().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateHostingAsync(It.IsAny<IUpdateSolutionHostingRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<MaxLengthResult>();
            var maxLengthResult = validationResult as MaxLengthResult;
            maxLengthResult.MaxLength.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await UpdatePublicCloud().ConfigureAwait(false));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateHostingAsync(It.IsAny<IUpdateSolutionHostingRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        private async Task<ISimpleResult> UpdatePublicCloud()
        {
            return await Context.UpdatePublicCloudHandler.Handle(
                new UpdatePublicCloudCommand(SolutionId, _dataMock.Object),
                new CancellationToken()).ConfigureAwait(false);
        }
    }
}
