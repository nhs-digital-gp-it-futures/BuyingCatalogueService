using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PrivateCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Hostings
{
    [TestFixture]
    internal sealed class UpdatePrivateCloudTests : HostingTestsBase
    {
        private const string SolutionId = "Sln1";
        private const string SummaryToken = "PrivateCloud.Summary";
        private const string LinkToken = "PrivateCloud.Link";
        private const string HostingModelToken = "PrivateCloud.HostingModel";
        private const string RequiresHscnToken = "PrivateCloud.RequiresHSCN";

        private Mock<IUpdatePrivateCloudData> _dataMock;
        private string _summary;
        private string _link;
        private string _hostingModel;
        private HashSet<string> _requiresHscn;

        [SetUp]
        public void Setup()
        {
            _summary = "A summary";
            _link = "A link";
            _hostingModel = "A hosting model";
            _requiresHscn = new HashSet<string> {"A requires string"};

        _dataMock = new Mock<IUpdatePrivateCloudData>();
            _dataMock.Setup(x => x.Summary).Returns(() => _summary);
            _dataMock.Setup(x => x.Link).Returns(() => _link);
            _dataMock.Setup(x => x.HostingModel).Returns(() => _hostingModel);
            _dataMock.Setup(x => x.RequiresHSCN).Returns(() => _requiresHscn);
        }

        [Test]
        public async Task ValidDataShouldUpdateNativeDesktopMemoryAndStorage()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("");
            var validationResult = await Update().ConfigureAwait(false);

            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once);
            Context.MockSolutionDetailRepository.Verify(x => x.UpdateHostingAsync(It.Is<IUpdateSolutionHostingRequest>(
                c =>
                    c.SolutionId == SolutionId &&
                        JToken.Parse(c.Hosting).SelectToken(SummaryToken).Value<string>() == _summary &&
                        JToken.Parse(c.Hosting).SelectToken(LinkToken).Value<string>() == _link &&
                        JToken.Parse(c.Hosting).SelectToken(HostingModelToken).Value<string>() == _hostingModel &&
                        JToken.Parse(c.Hosting).SelectToken(RequiresHscnToken).Value<string>() == _requiresHscn.First()
                ), It.IsAny<CancellationToken>()), Times.Once);
            validationResult.IsValid.Should().BeTrue();
        }

        [TestCase(500, 1000, 1001, "hosting-model")]
        [TestCase(500, 1001, 1000, "link")]
        [TestCase(501, 1000, 1000, "summary")]
        [TestCase(501, 1001, 1001, "summary", "link", "hosting-model")]
        public async Task MissingDataShouldReturnRequiredValidationResult(int summary, int link, int hosting, params string[] expected)
        {
            _summary = new string('a', summary);
            _link = new string('a', link);
            _hostingModel = new string('a', hosting);

            SetUpMockSolutionRepositoryGetByIdAsync();
            var validationResult = await Update().ConfigureAwait(false);

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
        public void InvalidSolutionIdShouldThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await Update().ConfigureAwait(false));
            Context.MockSolutionRepository.Verify(x => x.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Once);
            Context.MockSolutionDetailRepository.Verify(
                x => x.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never);
        }

        private async Task<ISimpleResult> Update()
        {
            return await Context.UpdatePrivateCloudHandler.Handle(
                new UpdatePrivateCloudCommand(SolutionId, _dataMock.Object),
                new CancellationToken()).ConfigureAwait(false);
        }
    }
}
