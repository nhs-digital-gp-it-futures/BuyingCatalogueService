using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionGetByIdTests
    {
        private TestContext _context;

        private readonly DateTime _lastUpdated = DateTime.Today;

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
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated.ToString);
            existingSolution.Setup(s => s.Description).Returns("Description");
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns("AboutUrl");
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'orem ipsum' } }");
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");
            existingSolution.Setup(s => s.IsFoundation).Returns(true);

            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");
            var capabilities2 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap2");

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

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);
            solution.Summary.Should().Be("Summary");
            solution.OrganisationName.Should().Be("OrganisationName");
            solution.Description.Should().Be("Description");
            solution.AboutUrl.Should().Be("AboutUrl");
            solution.Features.Should().BeEquivalentTo(new [] {"Marmite", "Jam", "Marmelade"});
            solution.ClientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            solution.ClientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            solution.ClientApplication.MobileResponsive.Should().BeTrue();
            solution.ClientApplication.Plugins.Required.Should().BeTrue();
            solution.ClientApplication.Plugins.AdditionalInformation.Should().Be("orem ipsum");
            solution.IsFoundation.Should().BeTrue();
            solution.Capabilities.Should().BeEquivalentTo(new[] {"cap1", "cap2"});
            solution.Contacts.Count().Should().Be(1);
            var contact = solution.Contacts.Single();
            contact.Name.Should().Be("Bob Bobbington");
            contact.Email.Should().Be(expectedContact.Email);
            contact.PhoneNumber.Should().Be(expectedContact.PhoneNumber);
            contact.Department.Should().Be(expectedContact.Department);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetEmptySolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated.ToString);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);
            existingSolution.Setup(s => s.OrganisationName).Returns((string)null);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();
            solution.ClientApplication.ClientApplicationTypes.Should().BeEmpty();
            solution.ClientApplication.BrowsersSupported.Should().BeEmpty();
            solution.ClientApplication.MobileResponsive.Should().BeNull();
            solution.ClientApplication.Plugins.Should().BeNull();

            solution.OrganisationName.Should().BeNull();
            solution.Capabilities.Should().BeEmpty();
            solution.Contacts.Count().Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated.ToString);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns((string)null);
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");

            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(new[] { capabilities1 });

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().Be("Summary");
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();
            solution.ClientApplication.ClientApplicationTypes.Should().BeEmpty();
            solution.ClientApplication.BrowsersSupported.Should().BeEmpty();
            solution.ClientApplication.MobileResponsive.Should().BeNull();
            solution.ClientApplication.Plugins.Should().BeNull();
            
            solution.OrganisationName.Should().Be("OrganisationName");
            solution.Capabilities.Should().BeEquivalentTo(new[] {"cap1"});
            solution.Contacts.Count().Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionWithMarketingDataById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated.ToString);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");

            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");
            var capabilities2 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap2");
            var capabilities3 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap3");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(new[] { capabilities1, capabilities2, capabilities3 });

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEquivalentTo(new[] { "Marmite", "Jam", "Marmelade" });

            solution.OrganisationName.Should().Be("OrganisationName");
            solution.Capabilities.Should().BeEquivalentTo(new[] {"cap1", "cap2", "cap3"});
            solution.Contacts.Count().Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionWithClientApplicationById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.LastUpdated).Returns(_lastUpdated.ToString);
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true, 'Plugins' : {'Required' : false, 'AdditionalInformation': null } }");
            existingSolution.Setup(s => s.OrganisationName).Returns("OrganisationName");
            existingSolution.Setup(s => s.IsFoundation).Returns(false);
            var capabilities1 = Mock.Of<ISolutionCapabilityListResult>(m => m.CapabilityId == new Guid() && m.CapabilityName == "cap1");

            _context.MockSolutionCapabilityRepository
                .Setup(r => r.ListSolutionCapabilities("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] {capabilities1});

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Name");
            solution.LastUpdated.Should().Be(_lastUpdated);

            solution.Summary.Should().BeNullOrEmpty();
            solution.Description.Should().BeNullOrEmpty();
            solution.AboutUrl.Should().BeNullOrEmpty();

            solution.Features.Should().BeEmpty();
            solution.ClientApplication.ClientApplicationTypes.Should().BeEquivalentTo(new[] { "browser-based", "native-mobile" });
            solution.ClientApplication.BrowsersSupported.Should().BeEquivalentTo(new[] { "Chrome", "Edge" });
            solution.ClientApplication.MobileResponsive.Should().BeTrue();

            solution.ClientApplication.Plugins.Required.Should().BeFalse();
            solution.ClientApplication.Plugins.AdditionalInformation.Should().BeNull();

            solution.OrganisationName.Should().Be("OrganisationName");
            solution.Capabilities.Should().BeEquivalentTo(new[] {"cap1"});
            solution.Contacts.Count().Should().Be(0);

            solution.IsFoundation.Should().BeFalse();
            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
