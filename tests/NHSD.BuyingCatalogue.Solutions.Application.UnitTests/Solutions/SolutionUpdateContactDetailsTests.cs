using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
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
            var result = await CallHandle(_existingSolutionId);
            result.IsValid.Should().BeTrue();
            result.MaxLength.Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(_existingSolutionId, It.Is<IEnumerable<IContact>>(c => VerifyContacts(c)),It.IsAny<CancellationToken>()), Times.Once);
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

            var result = await CallHandle(_existingSolutionId);
            result.IsValid.Should().BeFalse();
            result.MaxLength.Should().Contain("contact1-first-name");
            result.MaxLength.Should().Contain("contact1-last-name");
            result.MaxLength.Should().Contain("contact1-email-address");
            result.MaxLength.Should().Contain("contact1-department-name");
            result.MaxLength.Should().Contain("contact1-phone-number");
            result.MaxLength.Should().Contain("contact2-first-name");
            result.MaxLength.Should().Contain("contact2-last-name");
            result.MaxLength.Should().Contain("contact2-email-address");
            result.MaxLength.Should().Contain("contact2-department-name");
            result.MaxLength.Should().Contain("contact2-phone-number");

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(_existingSolutionId, It.IsAny<IEnumerable<IContact>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task NullDataShouldUpdateContacts()
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

            var result = await CallHandle(_existingSolutionId);
            result.IsValid.Should().BeTrue();
            result.MaxLength.Should().BeEmpty();

            _context.MockMarketingContactRepository.Verify(x => x.ReplaceContactsForSolution(_existingSolutionId, It.Is<IEnumerable<IContact>>(c => VerifyContacts(c)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task NullContactsShouldUpdate()
        {
            _contact1 = null;
            _contact2 = null;

            var result = await CallHandle(_existingSolutionId);
            result.IsValid.Should().BeTrue();
            result.MaxLength.Should().BeEmpty();

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

        private async Task<UpdateSolutionContactDetailsValidationResult> CallHandle(string solutionId)
        {
            var command = new UpdateSolutionContactDetailsCommand(solutionId, new UpdateSolutionContactDetailsViewModel{Contact1 = _contact1, Contact2 = _contact2});
            return await _context.UpdateSolutionContactDetailsHandler.Handle(command, new CancellationToken());
        }
    }
}
