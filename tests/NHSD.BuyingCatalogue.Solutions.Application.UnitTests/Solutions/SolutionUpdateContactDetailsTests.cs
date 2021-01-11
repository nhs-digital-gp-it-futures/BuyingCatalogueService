using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public class SolutionUpdateContactDetailsTests
    {
        private TestContext _context;

        private static readonly string _36CharacterString = new('a', 36);
        private static readonly string _256CharacterString = new('a', 256);
        private static readonly string _51CharacterString = new('a', 51);

        private UpdateSolutionContactViewModel _contact1;
        private UpdateSolutionContactViewModel _contact2;

        private const string ExistingSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln2";

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
            _contact1 = new UpdateSolutionContactViewModel
            {
                FirstName = "Bob",
                LastName = "Bobbington",
                Department = "Bobbing",
                Email = "bob@bob.bob",
                PhoneNumber = "123"
            };
            _contact2 = new UpdateSolutionContactViewModel
            {
                FirstName = "Betty",
                LastName = "Bettington",
                Department = "Betting",
                Email = "betty@betty.betty",
                PhoneNumber = "321"
            };
            _context.MockSolutionRepository.Setup(x => x.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _context.MockSolutionRepository.Setup(x => x.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateContactDetails()
        {
            var result = await CallHandle(ExistingSolutionId)
                .ConfigureAwait(false);
            result.IsValid.Should().BeTrue();
            result.Contact1Result.ToDictionary().Should().BeEmpty();
            result.Contact2Result.ToDictionary().Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(ExistingSolutionId, It.Is<IEnumerable<IContact>>(c => VerifyContacts(c)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InvalidDataShouldReturnError()
        {
            _contact1.FirstName = _36CharacterString;
            _contact1.LastName = _36CharacterString;
            _contact1.Department = _51CharacterString;
            _contact1.PhoneNumber = _36CharacterString;
            _contact1.Email = _256CharacterString;

            _contact2.FirstName = _36CharacterString;
            _contact2.LastName = _36CharacterString;
            _contact2.Department = _51CharacterString;
            _contact2.PhoneNumber = _36CharacterString;
            _contact2.Email = _256CharacterString;

            var result = await CallHandle(ExistingSolutionId).ConfigureAwait(false);
            result.IsValid.Should().BeFalse();

            result.Contact1Result.IsValid.Should().BeFalse();
            var contact1Result = result.Contact1Result.ToDictionary();
            contact1Result.Count.Should().Be(5);
            contact1Result["first-name"].Should().Be("maxLength");
            contact1Result["last-name"].Should().Be("maxLength");
            contact1Result["email-address"].Should().Be("maxLength");
            contact1Result["department-name"].Should().Be("maxLength");
            contact1Result["phone-number"].Should().Be("maxLength");

            result.Contact2Result.IsValid.Should().BeFalse();
            var contact2Result = result.Contact1Result.ToDictionary();
            contact2Result.Count.Should().Be(5);
            contact2Result["first-name"].Should().Be("maxLength");
            contact2Result["last-name"].Should().Be("maxLength");
            contact2Result["email-address"].Should().Be("maxLength");
            contact2Result["department-name"].Should().Be("maxLength");
            contact2Result["phone-number"].Should().Be("maxLength");

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(ExistingSolutionId, It.IsAny<IEnumerable<IContact>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task NullDataShouldRemoveContacts()
        {
            _contact1.FirstName = null;
            _contact1.LastName = null;
            _contact1.Department = null;
            _contact1.PhoneNumber = null;
            _contact1.Email = null;

            _contact2.FirstName = null;
            _contact2.LastName = null;
            _contact2.Department = null;
            _contact2.PhoneNumber = null;
            _contact2.Email = null;

            var result = await CallHandle(ExistingSolutionId)
                .ConfigureAwait(false);
            result.IsValid.Should().BeTrue();
            result.Contact1Result.ToDictionary().Should().BeEmpty();
            result.Contact2Result.ToDictionary().Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(ExistingSolutionId, It.Is<IEnumerable<IContact>>(c => !c.Any()), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task NullContactsShouldRemoveContacts()
        {
            _contact1 = null;
            _contact2 = null;

            var result = await CallHandle(ExistingSolutionId)
                .ConfigureAwait(false);
            result.IsValid.Should().BeTrue();
            result.Contact1Result.ToDictionary().Should().BeEmpty();
            result.Contact2Result.ToDictionary().Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(ExistingSolutionId, It.Is<IEnumerable<IContact>>(c => !c.Any()), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void InvalidSolutionIdShouldThrowException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => CallHandle(InvalidSolutionId));
        }

        [Test]
        public void CommandWithBadParamsThrowsNullExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new UpdateSolutionContactDetailsCommand(null, new UpdateSolutionContactDetailsViewModel()));
            Assert.Throws<ArgumentNullException>(() => new UpdateSolutionContactDetailsCommand("Hello", null));
        }
        private bool VerifyContacts(IEnumerable<IContact> contacts)
        {
            contacts.Count().Should().Be(2);
            contacts.First().Should().BeEquivalentTo(_contact1);
            contacts.Last().Should().BeEquivalentTo(_contact2);
            return true;
        }

        private async Task<ContactsMaxLengthResult> CallHandle(string solutionId)
        {
            var command = new UpdateSolutionContactDetailsCommand(solutionId, new UpdateSolutionContactDetailsViewModel { Contact1 = _contact1, Contact2 = _contact2 });
            return await _context.UpdateSolutionContactDetailsHandler.Handle(command, new CancellationToken())
                .ConfigureAwait(false);
        }
    }
}
