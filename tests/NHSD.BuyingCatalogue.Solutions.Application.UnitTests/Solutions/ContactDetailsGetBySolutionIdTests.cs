using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class ContactDetailsGetBySolutionIdTests
    {
        private TestContext _context;
        private const string SolutionId = "Sln1";
        private CancellationToken _cancellationToken;
        private List<IMarketingContactResult> _marketingContactResult;
        private bool _solutionExists;

        private static readonly IMarketingContactResult MarketingContact1 = Mock.Of<IMarketingContactResult>(m => m.Id ==1 &&
                                                                           m.SolutionId == SolutionId &&
                                                                           m.FirstName == "Bob" &&
                                                                           m.LastName == "Builder" &&
                                                                           m.Email == "bob@builder.com" &&
                                                                           m.Department == "building" &&
                                                                           m.PhoneNumber == "12345678901");

        private static readonly IMarketingContactResult MarketingContact2 = Mock.Of<IMarketingContactResult>(m => m.Id == 2 &&
                                                                           m.SolutionId == SolutionId &&
                                                                           m.FirstName == "Alice" &&
                                                                           m.LastName == "Wonderland" &&
                                                                           m.Email == "alice@wonderland.com" &&
                                                                           m.Department == "prescription" &&
                                                                           m.PhoneNumber == "0123412345");

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();
            _cancellationToken = new CancellationToken();

            _solutionExists = true;

            _context.MockSolutionRepository
                .Setup(r => r.CheckExists(SolutionId, _cancellationToken)).ReturnsAsync(() =>_solutionExists);

            _context.MockMarketingContactRepository
                .Setup(r => r.BySolutionIdAsync(SolutionId, _cancellationToken))
                .ReturnsAsync(() => _marketingContactResult);
            _marketingContactResult = new List<IMarketingContactResult>();
        }

        [Test]
        public async Task ShouldGetContactDetailsBySolutionId()
        {
            _marketingContactResult.Add(MarketingContact1);
            _marketingContactResult.Add(MarketingContact2);

            var marketingContact = (await _context.GetContactDetailBySolutionIdHandler.Handle(
                new GetContactDetailBySolutionIdQuery(SolutionId), _cancellationToken).ConfigureAwait(false)).ToArray();
      
            marketingContact.Count().Should().Be(2);

            marketingContact[0].FirstName.Should().Be("Bob");
            marketingContact[0].LastName.Should().Be("Builder");
            marketingContact[0].Email.Should().Be("bob@builder.com");
            marketingContact[0].Department.Should().Be("building");
            marketingContact[0].PhoneNumber.Should().Be("12345678901");

            marketingContact[1].FirstName.Should().Be("Alice");
            marketingContact[1].LastName.Should().Be("Wonderland");
            marketingContact[1].Email.Should().Be("alice@wonderland.com");
            marketingContact[1].Department.Should().Be("prescription");
            marketingContact[1].PhoneNumber.Should().Be("0123412345");
        }


        [Test]
        public void SolutionCheckFailsThrowsException()
        {
            _solutionExists = false;

            Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetContactDetailBySolutionIdHandler.Handle(new GetContactDetailBySolutionIdQuery(SolutionId),
                    _cancellationToken));
        }

        [Test]
        public async Task ContactDetailsAreEmptyList()
        {
            var marketingContact = (await _context.GetContactDetailBySolutionIdHandler.Handle(
                new GetContactDetailBySolutionIdQuery(SolutionId), _cancellationToken).ConfigureAwait(false)).ToArray();

            marketingContact.Length.Should().Be(0);
        }
    }
}
