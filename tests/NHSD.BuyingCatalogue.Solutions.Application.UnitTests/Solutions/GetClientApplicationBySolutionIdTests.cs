using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    class GetClientApplicationBySolutionIdTests
    {
        private TestContext _context;
        private string _solutionId;
        private CancellationToken _cancellationToken;
        private IClientApplicationResult _clientApplicationResult;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _solutionId = "Sln1";
            _cancellationToken = new CancellationToken();
            _context.MockSolutionDetailRepository
                .Setup(r => r.GetClientApplicationBySolutionIdAsync(_solutionId, _cancellationToken))
                .ReturnsAsync(() => _clientApplicationResult);
        }

        [Test]
        public async Task ShouldGetClientApplicationById()
        {
            _clientApplicationResult = Mock.Of<IClientApplicationResult>(r =>
                r.Id == _solutionId &&
                r.ClientApplication == "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'orem ipsum' }, 'HardwareRequirements': 'New Hardware', 'NativeMobileHardwareRequirements': 'New Native Mobile Hardware', 'NativeDesktopHardwareRequirements': 'New Native Desktop Hardware', 'AdditionalInformation': 'Some Additional Info', 'MobileFirstDesign': false, 'NativeMobileFirstDesign': false, 'MobileOperatingSystems': { 'OperatingSystems': ['Windows', 'Linux'], 'OperatingSystemsDescription': 'For windows only version 10' }, 'MobileConnectionDetails': { 'ConnectionType': ['3G', '4G'], 'Description': 'A description', 'MinimumConnectionSpeed': '1GBps' }, 'MobileMemoryAndStorage': { 'Description': 'A description', 'MinimumMemoryRequirement': '1GB' }, 'NativeMobileAdditionalInformation': 'native mobile additional info' }"
                );

            var clientApplication = await _context.GetClientApplicationBySolutionIdHandler.Handle(
                new GetClientApplicationBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);
            clientApplication.Should().NotBeNull();
            clientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            clientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            clientApplication.MobileResponsive.Should().BeTrue();
            clientApplication.Plugins.Required.Should().BeTrue();
            clientApplication.Plugins.AdditionalInformation.Should().Be("orem ipsum");
            clientApplication.NativeMobileAdditionalInformation.Should().Be("native mobile additional info");
            clientApplication.HardwareRequirements.Should().Be("New Hardware");
            clientApplication.NativeMobileHardwareRequirements.Should().Be("New Native Mobile Hardware");
            clientApplication.NativeDesktopHardwareRequirements.Should().Be("New Native Desktop Hardware");
            clientApplication.AdditionalInformation.Should().Be("Some Additional Info");
            clientApplication.MobileFirstDesign.Should().BeFalse();
            clientApplication.NativeMobileFirstDesign.Should().BeFalse();
            clientApplication.MobileOperatingSystems.OperatingSystems.Should()
                .BeEquivalentTo(new[] {"Windows", "Linux"});
            clientApplication.MobileOperatingSystems.OperatingSystemsDescription.Should()
                .Be("For windows only version 10");
            clientApplication.MobileConnectionDetails.ConnectionType.Should().BeEquivalentTo(new[] {"3G", "4G"});
            clientApplication.MobileConnectionDetails.Description.Should().Be("A description");
            clientApplication.MobileConnectionDetails.MinimumConnectionSpeed.Should().Be("1GBps");
            clientApplication.MobileMemoryAndStorage.Description.Should().Be("A description");
            clientApplication.MobileMemoryAndStorage.MinimumMemoryRequirement.Should().Be("1GB");
        }

        [Test]
        public async Task EmptyClientApplicationResultReturnsDefaultClientApplication()
        {
            _clientApplicationResult = Mock.Of<IClientApplicationResult>(r =>
                r.Id == _solutionId &&
                r.ClientApplication == null
                );

            var clientApplication = await _context.GetClientApplicationBySolutionIdHandler.Handle(
                new GetClientApplicationBySolutionIdQuery(_solutionId), _cancellationToken).ConfigureAwait(false);
            clientApplication.Should().NotBeNull();
            clientApplication.Should().BeEquivalentTo(new ClientApplicationDto());
        }

        [Test]
        public void NullClientApplicationResultThrowsNotFoundException()
        {
            _clientApplicationResult = null;

            Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetClientApplicationBySolutionIdHandler.Handle(
                new GetClientApplicationBySolutionIdQuery(_solutionId), _cancellationToken));
        }
    }
}
