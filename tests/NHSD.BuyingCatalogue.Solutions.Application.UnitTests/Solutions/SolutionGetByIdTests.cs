using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionGetByIdTests
    {
        private TestContext _context;

        private readonly DateTime _lastUpdated = DateTime.Today;

        private const string DateFormat = "dd/MM/yyyy";

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldGetSolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated);
            existingSolution.Setup(s => s.Description).Returns("Description");
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns("AboutUrl");
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.RoadMap).Returns("Some valid roadmap description");
            existingSolution.Setup(s => s.IntegrationsUrl).Returns("Some valid integrations url");
            existingSolution.Setup(s => s.ImplementationTimescales).Returns("Some valid implementation timescales description");
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'orem ipsum' }, 'MobileFirstDesign': true, 'MobileOperatingSystems': { 'OperatingSystems': ['Windows', 'Linux'], 'OperatingSystemsDescription': 'For windows only version 10' }, 'MobileConnectionDetails': { 'ConnectionType': ['3G', '4G'], 'Description': 'A description', 'MinimumConnectionSpeed': '1GBps' }, 'MobileThirdParty': { 'ThirdPartyComponents': 'Component', 'DeviceCapabilities': 'Capabilities'}, 'NativeMobileHardwareRequirements': 'Native Mobile Hardware', 'NativeDesktopHardwareRequirements': 'Native Desktop Hardware', 'NativeMobileAdditionalInformation': 'native mobile additional information', 'NativeDesktopMinimumConnectionSpeed': '6Mbps', 'NativeDesktopOperatingSystemsDescription':'native desktop operating systems description', 'NativeDesktopThirdParty': { 'ThirdPartyComponents': 'Components', 'DeviceCapabilities': 'Capabilities' }, 'NativeDesktopMemoryAndStorage': { 'MinimumMemoryRequirement': '512MB', 'StorageRequirementsDescription': '1024GB', 'MinimumCpu': '3.4GHz', 'RecommendedResolution': '800x600' }, 'NativeDesktopAdditionalInformation': 'some additional information' }");
            existingSolution.Setup(s => s.Hosting).Returns("{ 'PublicCloud': { 'Summary': 'Some summary', 'Link': 'some link', 'RequiresHSCN': 'It is required' } }");
            existingSolution.Setup(s => s.IsFoundation).Returns(true);

            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");
            var capabilities2 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap2");

            var mockSupplier = Mock.Of<ISupplierResult>(m =>
                m.Name == "supplier name" && m.Summary == "supplier summary" && m.Url == "supplierUrl");

            var mockDocument = new Mock<IDocumentResult>();
            mockDocument.Setup(s => s.RoadMapDocumentName).Returns("RoadMap.pdf");
            mockDocument.Setup(s => s.IntegrationDocumentName).Returns("Integration.pdf");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(new []{capabilities1, capabilities2});
            var expectedContact = Mock.Of<IMarketingContactResult>(c =>
                c.Id == 1 &&
                c.SolutionId == "Sln1" &&
                c.FirstName == "Bob" &&
                c.LastName == "Bobbington" &&
                c.Email == "Test");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);
            _context.MockMarketingContactRepository
                .Setup(r => r.BySolutionIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[]{expectedContact});

            _context.MockSupplierRepository.Setup(r => r.GetSupplierBySolutionIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(mockSupplier);

            _context.MockDocumentRepository
                .Setup(r => r.GetDocumentResultBySolutionIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockDocument.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()).ConfigureAwait(false);

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);
            solution.Summary.Should().Be("Summary");

            solution.Description.Should().Be("Description");
            solution.AboutUrl.Should().Be("AboutUrl");

            solution.Features.Should().BeEquivalentTo(new [] {"Marmite", "Jam", "Marmelade"});

            solution.ImplementationTimescales.Description.Should().Be("Some valid implementation timescales description");
            solution.Integrations.Url.Should().Be("Some valid integrations url");
            solution.Integrations.DocumentName.Should().Be("Integration.pdf");

            solution.RoadMap.Summary.Should().Be("Some valid roadmap description");
            solution.RoadMap.DocumentName.Should().Be("RoadMap.pdf");

            solution.ClientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            solution.ClientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            solution.ClientApplication.MobileResponsive.Should().BeTrue();
            solution.ClientApplication.Plugins.Required.Should().BeTrue();
            solution.ClientApplication.Plugins.AdditionalInformation.Should().Be("orem ipsum");
            solution.ClientApplication.NativeMobileAdditionalInformation.Should().Be("native mobile additional information");
            solution.ClientApplication.MobileFirstDesign.Should().BeTrue();
            solution.ClientApplication.MobileOperatingSystems.OperatingSystems.Should()
                .BeEquivalentTo(new[] {"Windows", "Linux"});
            solution.ClientApplication.MobileOperatingSystems.OperatingSystemsDescription.Should()
                .Be("For windows only version 10");
            solution.ClientApplication.MobileConnectionDetails.ConnectionType.Should().BeEquivalentTo(new[] { "3G", "4G" });
            solution.ClientApplication.MobileConnectionDetails.Description.Should().Be("A description");
            solution.ClientApplication.MobileConnectionDetails.MinimumConnectionSpeed.Should().Be("1GBps");
            solution.ClientApplication.MobileThirdParty.ThirdPartyComponents.Should().Be("Component");
            solution.ClientApplication.MobileThirdParty.DeviceCapabilities.Should().Be("Capabilities");
            solution.ClientApplication.NativeMobileHardwareRequirements.Should().Be("Native Mobile Hardware");
            solution.ClientApplication.NativeDesktopHardwareRequirements.Should().Be("Native Desktop Hardware");
            solution.ClientApplication.NativeDesktopMinimumConnectionSpeed.Should().Be("6Mbps");
            solution.ClientApplication.NativeDesktopOperatingSystemsDescription.Should()
                .Be("native desktop operating systems description");
            solution.ClientApplication.NativeDesktopThirdParty.ThirdPartyComponents.Should().Be("Components");
            solution.ClientApplication.NativeDesktopThirdParty.DeviceCapabilities.Should().Be("Capabilities");
            solution.ClientApplication.NativeDesktopMemoryAndStorage.MinimumMemoryRequirement.Should().Be("512MB");
            solution.ClientApplication.NativeDesktopMemoryAndStorage.StorageRequirementsDescription.Should()
                .Be("1024GB");
            solution.ClientApplication.NativeDesktopMemoryAndStorage.MinimumCpu.Should().Be("3.4GHz");
            solution.ClientApplication.NativeDesktopMemoryAndStorage.RecommendedResolution.Should().Be("800x600");
            solution.ClientApplication.NativeDesktopAdditionalInformation.Should().Be("some additional information");

            solution.Hosting.Should().NotBeNull();
            solution.Hosting.PublicCloud.Should().NotBeNull();
            solution.Hosting.PublicCloud.Summary.Should().Be("Some summary");
            solution.Hosting.PublicCloud.Link.Should().Be("some link");
            solution.Hosting.PublicCloud.RequiresHSCN.Should().Be("It is required");

            solution.IsFoundation.Should().BeTrue();
            solution.Capabilities.Should().HaveCount(2);
            solution.Capabilities.Should()
                .BeEquivalentTo(
                    new[]
                    {
                        Mock.Of<IClaimedCapability>(cc => cc.Name == "cap1"),
                        Mock.Of<IClaimedCapability>(cc => cc.Name == "cap2")
                    }, config => config.ComparingByMembers<IClaimedCapability>());


            solution.Contacts.Count().Should().Be(1);
            var contact = solution.Contacts.Single();
            contact.Name.Should().Be("Bob Bobbington");
            contact.Email.Should().Be(expectedContact.Email);
            contact.PhoneNumber.Should().Be(expectedContact.PhoneNumber);
            contact.Department.Should().Be(expectedContact.Department);

            solution.Supplier.Summary.Should().Be(mockSupplier.Summary);
            solution.Supplier.Url.Should().Be(mockSupplier.Url);

            solution.SupplierName.Should().Be(mockSupplier.Name);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetEmptySolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.RoadMap).Returns((string)null);
            existingSolution.Setup(s => s.IntegrationsUrl).Returns((string)null);
            existingSolution.Setup(s => s.ImplementationTimescales).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()).ConfigureAwait(false);

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();
            solution.Features.Should().BeEmpty();

            solution.RoadMap.DocumentName.Should().BeNullOrEmpty();
            solution.RoadMap.Summary.Should().BeNullOrEmpty();

            solution.ImplementationTimescales.Description.Should().BeNullOrEmpty();
            solution.Integrations.Url.Should().BeNullOrEmpty();
            solution.Integrations.DocumentName.Should().BeNullOrEmpty();

            solution.ClientApplication.ClientApplicationTypes.Should().BeEmpty();
            solution.ClientApplication.BrowsersSupported.Should().BeEmpty();
            solution.ClientApplication.MobileResponsive.Should().BeNull();
            solution.ClientApplication.Plugins.Should().BeNull();
            solution.ClientApplication.MobileFirstDesign.Should().BeNull();
            solution.ClientApplication.MobileOperatingSystems.Should().BeNull();
            solution.ClientApplication.MobileConnectionDetails.Should().BeNull();
            solution.ClientApplication.MobileThirdParty.Should().BeNull();
            solution.ClientApplication.NativeMobileHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopMinimumConnectionSpeed.Should().BeNull();
            solution.ClientApplication.NativeDesktopOperatingSystemsDescription.Should().BeNull();
            solution.ClientApplication.NativeDesktopThirdParty.Should().BeNull();
            solution.ClientApplication.NativeDesktopMemoryAndStorage.Should().BeNull();
            solution.ClientApplication.NativeDesktopAdditionalInformation.Should().BeNull();

            solution.SupplierName.Should().BeNull();
            solution.Capabilities.Should().BeEmpty();
            solution.Contacts.Count().Should().Be(0);

            solution.Supplier.Summary.Should().BeNull();
            solution.Supplier.Url.Should().BeNull();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.RoadMap).Returns((string)null);
            existingSolution.Setup(s => s.IntegrationsUrl).Returns((string)null);
            existingSolution.Setup(s => s.ImplementationTimescales).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);

            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(new[] { capabilities1 });

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()).ConfigureAwait(false);

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().Be("Summary");
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();

            solution.ImplementationTimescales.Description.Should().BeNullOrEmpty();
            solution.Integrations.Url.Should().BeNullOrEmpty();
            solution.Integrations.DocumentName.Should().BeNullOrEmpty();

            solution.RoadMap.Summary.Should().BeNullOrEmpty();
            solution.RoadMap.DocumentName.Should().BeNullOrEmpty();

            solution.ClientApplication.ClientApplicationTypes.Should().BeEmpty();
            solution.ClientApplication.BrowsersSupported.Should().BeEmpty();
            solution.ClientApplication.MobileResponsive.Should().BeNull();
            solution.ClientApplication.Plugins.Should().BeNull();
            solution.ClientApplication.MobileFirstDesign.Should().BeNull();
            solution.ClientApplication.MobileOperatingSystems.Should().BeNull();
            solution.ClientApplication.MobileConnectionDetails.Should().BeNull();
            solution.ClientApplication.MobileThirdParty.Should().BeNull();
            solution.ClientApplication.NativeMobileHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopOperatingSystemsDescription.Should().BeNull();
            solution.ClientApplication.NativeDesktopHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopMinimumConnectionSpeed.Should().BeNull();
            solution.ClientApplication.NativeDesktopThirdParty.Should().BeNull();
            solution.ClientApplication.NativeDesktopMemoryAndStorage.Should().BeNull();
            solution.ClientApplication.NativeDesktopAdditionalInformation.Should().BeNull();
            solution.Capabilities.Should().HaveCount(1);
            solution.Capabilities.Should().BeEquivalentTo(new[] {Mock.Of<IClaimedCapability>(cc=>cc.Name=="cap1") }, config=>config.ComparingByMembers<IClaimedCapability>());
            solution.Contacts.Should().HaveCount(0);

            solution.Supplier.Summary.Should().BeNull();
            solution.Supplier.Url.Should().BeNull();

            solution.SupplierName.Should().BeNull();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionWithMarketingDataById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.RoadMap).Returns((string)null);
            existingSolution.Setup(s => s.IntegrationsUrl).Returns((string)null);
            existingSolution.Setup(s => s.ImplementationTimescales).Returns((string)null);

            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");
            var capabilities2 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap2");
            var capabilities3 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap3");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(new[] { capabilities1, capabilities2, capabilities3 });

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()).ConfigureAwait(false);

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEquivalentTo(new[] { "Marmite", "Jam", "Marmelade" });

            solution.RoadMap.Summary.Should().BeNullOrEmpty();

            solution.ImplementationTimescales.Description.Should().BeNullOrEmpty();

            solution.Integrations.Url.Should().BeNullOrEmpty();
            solution.Integrations.DocumentName.Should().BeNullOrEmpty();

            solution.Capabilities.Should().HaveCount(3);
            solution.Capabilities.Should()
                .BeEquivalentTo(
                    new[]
                    {
                        Mock.Of<IClaimedCapability>(cc => cc.Name == "cap1"),
                        Mock.Of<IClaimedCapability>(cc => cc.Name == "cap2"),
                        Mock.Of<IClaimedCapability>(cc => cc.Name == "cap3")
                    }, config => config.ComparingByMembers<IClaimedCapability>());


            solution.Contacts.Count().Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase("01/01/0001", "01/01/0001", "01/01/0001","01/01/0001")]
        [TestCase("31/12/9999", "31/12/9999", "31/12/9999", "31/12/9999")]
        [TestCase("01/03/2019", "02/05/2018", "28/02/2019", "01/03/2019")]
        [TestCase("15/03/2020", "16/03/2020", "17/03/2019", "16/03/2020")]
        [TestCase("24/12/2019", "31/12/2018", "25/12/2019", "25/12/2019")]
        public void ShouldGetLastUpdatedForSolution(string existingSolutionDate, string marketingContact1Date, string marketingContact2Date, string expectedDate)
        {
            var dateTimeExpected = DateTime.ParseExact(expectedDate, DateFormat, CultureInfo.InvariantCulture);
            var existingSolution = Mock.Of<ISolutionResult>(s => s.LastUpdated == (DateTime.ParseExact(existingSolutionDate, DateFormat, CultureInfo.InvariantCulture)));
            var existingSupplier = Mock.Of<ISupplierResult>();

            var contact1Date = DateTime.ParseExact(marketingContact1Date, DateFormat, CultureInfo.InvariantCulture);
            var contact2Date = DateTime.ParseExact(marketingContact2Date, DateFormat, CultureInfo.InvariantCulture);

            var existingMarketingContactResult = new List<IMarketingContactResult>
            {
                Mock.Of<IMarketingContactResult>(s => s.LastUpdated == contact1Date),
                Mock.Of<IMarketingContactResult>(s => s.LastUpdated == contact2Date),
            };

            var solution = new Solution(existingSolution, new List<ISolutionCapabilityListResult>(),
                existingMarketingContactResult, existingSupplier,null);

            solution.LastUpdated.Should().Be(dateTimeExpected);
        }

        [TestCase("01/01/0001", "01/01/0001")]
        [TestCase("31/12/9999", "31/12/9999")]
        [TestCase("01/03/2025", "01/03/2025")]
        [TestCase("15/03/2020", "15/03/2020")]
        public void ShouldGetLastUpdatedForSolutionWithNoMarketingContacts(string existingSolutionDate, string expectedDate)
        {
            var dateTimeExpected = DateTime.ParseExact(expectedDate, DateFormat, CultureInfo.InvariantCulture);
            var existingSolution = Mock.Of<ISolutionResult>(s => s.LastUpdated == (DateTime.ParseExact(existingSolutionDate, DateFormat, CultureInfo.InvariantCulture)));
            var existingSupplier = Mock.Of<ISupplierResult>();

            var solution = new Solution(existingSolution, new List<ISolutionCapabilityListResult>(),
                new List<IMarketingContactResult>(), existingSupplier, null);

            solution.LastUpdated.Should().Be(dateTimeExpected);
        }

        [Test]
        public async Task ShouldGetPartialSolutionWithClientApplicationById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.RoadMap).Returns((string)null);
            existingSolution.Setup(s => s.IntegrationsUrl).Returns((string)null);
            existingSolution.Setup(s => s.ImplementationTimescales).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true, 'Plugins' : {'Required' : false, 'AdditionalInformation': null }, 'MobileFirstDesign': false, 'MobileOperatingSystems': { 'OperatingSystems': ['Windows'], 'OperatingSystemsDescription': null }, 'MobileConnectionDetails': { 'ConnectionType': ['3G', '4G'], 'Description': 'A description', 'MinimumConnectionSpeed': '1GBps' }, 'MobileThirdParty': { 'ThirdPartyComponents': 'Component' }, 'NativeDesktopMinimumConnectionSpeed': '3 Mbps', 'NativeDesktopThirdParty': { 'ThirdPartyComponents': 'New Components' }, 'NativeDesktopMemoryAndStorage': { 'MinimumMemoryRequirement': '512MB', 'StorageRequirementsDescription': '1024GB', 'MinimumCpu': '3.4GHz' } }");
            existingSolution.Setup(s => s.IsFoundation).Returns(false);
            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] {capabilities1});

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()).ConfigureAwait(false);

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();

            solution.ImplementationTimescales.Description.Should().BeNullOrEmpty();
            solution.Integrations.Url.Should().BeNullOrEmpty();
            solution.Integrations.DocumentName.Should().BeNullOrEmpty();

            solution.RoadMap.Summary.Should().BeNullOrEmpty();
            solution.RoadMap.DocumentName.Should().BeNullOrEmpty();

            solution.ClientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            solution.ClientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            solution.ClientApplication.MobileResponsive.Should().BeTrue();
            solution.ClientApplication.MobileFirstDesign.Should().BeFalse();

            solution.ClientApplication.Plugins.Required.Should().BeFalse();
            solution.ClientApplication.Plugins.AdditionalInformation.Should().BeNull();

            solution.ClientApplication.MobileOperatingSystems.OperatingSystems.Should()
                .BeEquivalentTo(new[] {"Windows"});
            solution.ClientApplication.MobileOperatingSystems.OperatingSystemsDescription.Should().BeNull();

            solution.ClientApplication.MobileConnectionDetails.ConnectionType.Should().BeEquivalentTo(new[] { "3G", "4G" });
            solution.ClientApplication.MobileConnectionDetails.Description.Should().Be("A description");
            solution.ClientApplication.MobileConnectionDetails.MinimumConnectionSpeed.Should().Be("1GBps");

            solution.ClientApplication.MobileThirdParty.ThirdPartyComponents.Should().Be("Component");
            solution.ClientApplication.MobileThirdParty.DeviceCapabilities.Should().BeNull();

            solution.ClientApplication.NativeMobileHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopOperatingSystemsDescription.Should().BeNull();

            solution.ClientApplication.NativeDesktopMinimumConnectionSpeed.Should().Be("3 Mbps");

            solution.ClientApplication.NativeDesktopThirdParty.ThirdPartyComponents.Should().Be("New Components");
            solution.ClientApplication.NativeDesktopThirdParty.DeviceCapabilities.Should().BeNull();

            solution.ClientApplication.NativeDesktopMemoryAndStorage.MinimumMemoryRequirement.Should().Be("512MB");
            solution.ClientApplication.NativeDesktopMemoryAndStorage.StorageRequirementsDescription.Should()
                .Be("1024GB");
            solution.ClientApplication.NativeDesktopMemoryAndStorage.MinimumCpu.Should().Be("3.4GHz");
            solution.ClientApplication.NativeDesktopMemoryAndStorage.RecommendedResolution.Should().BeNull();
            solution.ClientApplication.NativeDesktopAdditionalInformation.Should().BeNull();

            solution.Capabilities.Should().HaveCount(1);
            solution.Capabilities.Should()
                .BeEquivalentTo(
                    new[]
                    {
                        Mock.Of<IClaimedCapability>(cc => cc.Name == "cap1")
                    }, config => config.ComparingByMembers<IClaimedCapability>());

            solution.Contacts.Count().Should().Be(0);

            solution.IsFoundation.Should().BeFalse();
            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionByIdWithSupplier()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.RoadMap).Returns((string)null);
            existingSolution.Setup(s => s.IntegrationsUrl).Returns((string)null);
            existingSolution.Setup(s => s.ImplementationTimescales).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);

            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");

            var mockSupplier = Mock.Of<ISupplierResult>(m =>
                m.Name == "supplier name" && m.Summary == "supplier summary" && m.Url == "supplierUrl");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(new[] { capabilities1 });

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);
            _context.MockSupplierRepository.Setup(r => r.GetSupplierBySolutionIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(mockSupplier);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()).ConfigureAwait(false);

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().Be("Summary");
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();

            solution.ImplementationTimescales.Description.Should().BeNullOrEmpty();

            solution.Integrations.Url.Should().BeNullOrEmpty();
            solution.Integrations.DocumentName.Should().BeNullOrEmpty();

            solution.RoadMap.Summary.Should().BeNullOrEmpty();
            solution.RoadMap.DocumentName.Should().BeNullOrEmpty();

            solution.ClientApplication.ClientApplicationTypes.Should().BeEmpty();
            solution.ClientApplication.BrowsersSupported.Should().BeEmpty();
            solution.ClientApplication.MobileResponsive.Should().BeNull();
            solution.ClientApplication.Plugins.Should().BeNull();
            solution.ClientApplication.MobileFirstDesign.Should().BeNull();
            solution.ClientApplication.MobileOperatingSystems.Should().BeNull();
            solution.ClientApplication.MobileConnectionDetails.Should().BeNull();
            solution.ClientApplication.MobileThirdParty.Should().BeNull();
            solution.ClientApplication.NativeMobileHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopOperatingSystemsDescription.Should().BeNull();
            solution.ClientApplication.NativeDesktopHardwareRequirements.Should().BeNull();
            solution.ClientApplication.NativeDesktopMinimumConnectionSpeed.Should().BeNull();
            solution.ClientApplication.NativeDesktopThirdParty.Should().BeNull();
            solution.ClientApplication.NativeDesktopMemoryAndStorage.Should().BeNull();
            solution.ClientApplication.NativeDesktopAdditionalInformation.Should().BeNull();

            solution.Capabilities.Should().HaveCount(1);
            solution.Capabilities.Should()
                .BeEquivalentTo(
                    new[]
                    {
                        Mock.Of<IClaimedCapability>(cc => cc.Name == "cap1")
                    }, config => config.ComparingByMembers<IClaimedCapability>());
            solution.Contacts.Should().HaveCount(0);

            solution.Supplier.Summary.Should().Be(mockSupplier.Summary);
            solution.Supplier.Url.Should().Be(mockSupplier.Url);

            solution.SupplierName.Should().Be(mockSupplier.Name);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
