using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public class SolutionUpdateContactDetailsTests
    {
        private TestContext _context;

        private string _reallyLongString = "IAmAReallyLongStringThatShouldBreakAllValidationExceptEmailSoJustDuplicateMeThreeTimesToBreakThat";

        private UpdateSolutionContactViewModel _contact1;
        private UpdateSolutionContactViewModel _contact2;

        private string _existingSolutionId = "Sln1";
        private string _invalidSolutionId = "Sln2";

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
            _context.MockSolutionRepository.Setup(x => x.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _context.MockSolutionRepository.Setup(x => x.CheckExists(_invalidSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateContactDetails()
        {
            var result = await CallHandle(_existingSolutionId)
                .ConfigureAwait(false);
            result.IsValid.Should().BeTrue();
            result.Contact1Result.MaxLength.Should().BeEmpty();
            result.Contact2Result.MaxLength.Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(_existingSolutionId, It.Is<IEnumerable<IContact>>(c => VerifyContacts(c)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InvalidDataShouldReturnError()
        {
            _contact1.FirstName = _reallyLongString;
            _contact1.LastName = _reallyLongString;
            _contact1.Department = _reallyLongString;
            _contact1.PhoneNumber = _reallyLongString;
            _contact1.Email = _reallyLongString + _reallyLongString + _reallyLongString;

            _contact2.FirstName = _reallyLongString;
            _contact2.LastName = _reallyLongString;
            _contact2.Department = _reallyLongString;
            _contact2.PhoneNumber = _reallyLongString;
            _contact2.Email = _reallyLongString + _reallyLongString + _reallyLongString;

            var result = await CallHandle(_existingSolutionId).ConfigureAwait(false);
            result.IsValid.Should().BeFalse();

            result.Contact1Result.IsValid.Should().BeFalse();
            result.Contact1Result.MaxLength.Should().Contain("first-name");
            result.Contact1Result.MaxLength.Should().Contain("last-name");
            result.Contact1Result.MaxLength.Should().Contain("email-address");
            result.Contact1Result.MaxLength.Should().Contain("department-name");
            result.Contact1Result.MaxLength.Should().Contain("phone-number");

            result.Contact2Result.IsValid.Should().BeFalse();
            result.Contact2Result.MaxLength.Should().Contain("first-name");
            result.Contact2Result.MaxLength.Should().Contain("last-name");
            result.Contact2Result.MaxLength.Should().Contain("email-address");
            result.Contact2Result.MaxLength.Should().Contain("department-name");
            result.Contact2Result.MaxLength.Should().Contain("phone-number");

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(_existingSolutionId, It.IsAny<IEnumerable<IContact>>(), It.IsAny<CancellationToken>()), Times.Never);
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

            var result = await CallHandle(_existingSolutionId)
                .ConfigureAwait(false);
            result.IsValid.Should().BeTrue();
            result.Contact1Result.MaxLength.Should().BeEmpty();
            result.Contact2Result.MaxLength.Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(_existingSolutionId, It.Is<IEnumerable<IContact>>(c => !c.Any()), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task NullContactsShouldRemoveContacts()
        {
            _contact1 = null;
            _contact2 = null;

            var result = await CallHandle(_existingSolutionId)
                .ConfigureAwait(false);
            result.IsValid.Should().BeTrue();
            result.Contact1Result.MaxLength.Should().BeEmpty();
            result.Contact2Result.MaxLength.Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(_existingSolutionId, It.Is<IEnumerable<IContact>>(c => !c.Any()), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void InvalidSolutionIdShouldThrowException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => CallHandle(_invalidSolutionId));
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
