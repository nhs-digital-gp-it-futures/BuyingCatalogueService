using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateContactDetailsTests
    {
        private const string ExistingSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln2";

        private static readonly string CharacterStringLength36 = new('a', 36);
        private static readonly string CharacterStringLength256 = new('a', 256);
        private static readonly string CharacterStringLength51 = new('a', 51);

        private TestContext context;
        private UpdateSolutionContactViewModel contact1;
        private UpdateSolutionContactViewModel contact2;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
            contact1 = new UpdateSolutionContactViewModel
            {
                FirstName = "Bob",

                // ReSharper disable once StringLiteralTypo
                LastName = "Bobbington",
                Department = "Bobbing",
                Email = "bob@bob.bob",
                PhoneNumber = "123",
            };

            contact2 = new UpdateSolutionContactViewModel
            {
                FirstName = "Betty",

                // ReSharper disable once StringLiteralTypo
                LastName = "Bettington",
                Department = "Betting",
                Email = "betty@betty.betty",
                PhoneNumber = "321",
            };

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateContactDetails()
        {
            var result = await CallHandle(ExistingSolutionId);

            result.IsValid.Should().BeTrue();
            result.Contact1Result.ToDictionary().Should().BeEmpty();
            result.Contact2Result.ToDictionary().Should().BeEmpty();

            context.MockMarketingContactRepository.Verify(r => r.ReplaceContactsForSolution(
                ExistingSolutionId,
                It.Is<IEnumerable<IContact>>(c => VerifyContacts(c)),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task InvalidDataShouldReturnError()
        {
            contact1.FirstName = CharacterStringLength36;
            contact1.LastName = CharacterStringLength36;
            contact1.Department = CharacterStringLength51;
            contact1.PhoneNumber = CharacterStringLength36;
            contact1.Email = CharacterStringLength256;

            contact2.FirstName = CharacterStringLength36;
            contact2.LastName = CharacterStringLength36;
            contact2.Department = CharacterStringLength51;
            contact2.PhoneNumber = CharacterStringLength36;
            contact2.Email = CharacterStringLength256;

            var result = await CallHandle(ExistingSolutionId);
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

            Expression<Func<IMarketingContactRepository, Task>> expression = r => r.ReplaceContactsForSolution(
                ExistingSolutionId,
                It.IsAny<IEnumerable<IContact>>(),
                It.IsAny<CancellationToken>());

            context.MockMarketingContactRepository.Verify(expression, Times.Never());
        }

        [Test]
        public async Task NullDataShouldRemoveContacts()
        {
            contact1.FirstName = null;
            contact1.LastName = null;
            contact1.Department = null;
            contact1.PhoneNumber = null;
            contact1.Email = null;

            contact2.FirstName = null;
            contact2.LastName = null;
            contact2.Department = null;
            contact2.PhoneNumber = null;
            contact2.Email = null;

            var result = await CallHandle(ExistingSolutionId);

            result.IsValid.Should().BeTrue();
            result.Contact1Result.ToDictionary().Should().BeEmpty();
            result.Contact2Result.ToDictionary().Should().BeEmpty();

            context.MockMarketingContactRepository.Verify(r => r.ReplaceContactsForSolution(
                ExistingSolutionId,
                It.Is<IEnumerable<IContact>>(c => !c.Any()),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task NullContactsShouldRemoveContacts()
        {
            contact1 = null;
            contact2 = null;

            var result = await CallHandle(ExistingSolutionId);

            result.IsValid.Should().BeTrue();
            result.Contact1Result.ToDictionary().Should().BeEmpty();
            result.Contact2Result.ToDictionary().Should().BeEmpty();

            context.MockMarketingContactRepository.Verify(r => r.ReplaceContactsForSolution(
                ExistingSolutionId,
                It.Is<IEnumerable<IContact>>(c => !c.Any()),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public void InvalidSolutionIdShouldThrowException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => CallHandle(InvalidSolutionId));
        }

        [Test]
        public void CommandWithBadParamsThrowsNullExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new UpdateSolutionContactDetailsCommand(
                null,
                new UpdateSolutionContactDetailsViewModel()));

            Assert.Throws<ArgumentNullException>(() => _ = new UpdateSolutionContactDetailsCommand("Hello", null));
        }

        private bool VerifyContacts(IEnumerable<IContact> contacts)
        {
            var contactList = contacts.ToList();

            contactList.Count.Should().Be(2);
            contactList.First().Should().BeEquivalentTo(contact1);
            contactList.Last().Should().BeEquivalentTo(contact2);

            return true;
        }

        private async Task<ContactsMaxLengthResult> CallHandle(string solutionId)
        {
            var command = new UpdateSolutionContactDetailsCommand(
                solutionId,
                new UpdateSolutionContactDetailsViewModel { Contact1 = contact1, Contact2 = contact2 });

            return await context.UpdateSolutionContactDetailsHandler.Handle(command, CancellationToken.None);
        }
    }
}
